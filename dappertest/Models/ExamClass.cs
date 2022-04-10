using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;


namespace dapper.Models
{
    public class ExamClass
    {
    }

    /// <summary>
    ///     model class
    /// </summary>
    public class Question
    {
        public int Id { get; set; }

        [Required] public string Title { get; set; }
    }

    public class QuestionDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(4000, ErrorMessage = "over 4000 words")]
        public string Title { get; set; }
    }

    public interface IQuestionRepository
    {
        Question Add(Question model);
        List<Question> GetAll();
        Question GetById(int id);
        Question Update(Question model);
        void Remove(int id);
        List<Question> GetAllWithPaging(int pageIndex, int pageSize = 10);
        int GetRecordCount();
    }

    public class QuestionRepository : IQuestionRepository
    {
        private readonly IConfiguration _config;
        private readonly IDbConnection _dbConnection;

        public QuestionRepository(IConfiguration config)
        {
            _config = config;
            _dbConnection = new SqliteConnection(
                _config.GetSection("ConnectionStrings")
                    .GetSection("DefaultConnection").Value);
        }

        public Question Add(Question model)
        {
            var sql = @"Insert into Questions (Title) values(@Title);
                           Select Cast(SCOPE_Identity() AS) int ";
            var id = _dbConnection.Query<int>(sql).Single();
            model.Id = id;
            return model;
        }

        public List<Question> GetAll()
        {
            var sql = @"Select * From Questions ORDER by Id DESC";
            return _dbConnection.Query<Question>(sql).ToList();
        }

        public Question GetById(int id)
        {
            var query = "Select * from Questions where Id =@id";
            return _dbConnection.Query<Question>(query, new { Id = id }).Single();
        }

        public Question Update(Question model)
        {
            var query = "Update Questions Set Title = @Title where Id = @Id";
            _dbConnection.Execute(query, model);
            return model;
        }

        public void Remove(int id)
        {
            var query = "Delete From Questions where Id = @id";
            _dbConnection.Execute(query, new { Id = id });
        }

        public List<Question> GetAllWithPaging(int pageIndex, int pageSize = 10)
        {
            var sql = @"
            select Id, Title
            from (
                 select Row_Number() Over (Order By Id DESC) as RowNumbers, Id, Title
                 from Qustions
            ) As tempRowTalbes
            where RowNumbers between (@PageIndex * @PageSize +1) and (@PageIndex +1)*@PageSize";
            return _dbConnection.Query<Question>(sql, new { PageIndex = pageIndex, PageSize = pageSize }).ToList();
        }

        public int GetRecordCount()
        {
            var query = "Select Count(*) from Questions";
            return _dbConnection.Query<int>(query).FirstOrDefault();
        }
    }

    [Route("api/[controller]")]
    public class QuestionSerivceController : Controller
    {
        private readonly IQuestionRepository _repository;

        public QuestionSerivceController(IQuestionRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var models = _repository.GetAll();
                if (models == null) return NotFound("No data found");

                return Ok(models);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult Get(int id)
        {
            try
            {
                var models = _repository.GetById(id);
                if (models == null) return NotFound("No data found");

                return Ok(models);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Produces("application/json", Type = typeof(QuestionDTO))]
        [Consumes("application/json")]
        public IActionResult Post([FromBody] QuestionDTO model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            try
            {
                if (model.Title == null || model.Title.Length < 1)
                {
                    ModelState.AddModelError("Title", "enter question");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newModel = new Question { Id = model.Id, Title = model.Title };
                var m = _repository.Add(newModel);

                if (DateTime.Now.Second % 2 == 0)
                {
                    return CreatedAtRoute("GetById", new { id = m.Id }, m);
                }
                else
                {
                    var uri = Url.Link("GetById", new { id = m.Id });
                    return Created(uri, m);
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody] Question model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            try
            {
                var oldModel = _repository.GetById(id);
                if (oldModel == null)
                {
                    return NotFound("No data found");
                }

                model.Id = id;
                _repository.Update(model);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {

                var oldModel = _repository.GetById(id);
                if (oldModel == null)
                {
                    return NotFound("No data founc");
                }
                _repository.Remove(id);
                return NoContent();
            }
            catch (Exception e)
            {
                BadRequest();
            }

            return Ok();
        }

    }
}