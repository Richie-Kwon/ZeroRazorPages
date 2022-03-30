using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Formats.Asn1;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICRUD.Models
{
    /// <summary>
    ///Model class 
    /// </summary>
    public class Five
    {
        public int Id { get; set; }

        [Required] public string Note { get; set; }
    }

    public interface IFiveRepository
    {
        Five Add(Five model);
        Five GetById(int id);
        Five Update(Five model);
        void Remove(int id);
        List<Five> GetAll();
        List<Five> GetAllWithPaging(int pageIndex, int pageSize = 10);
    }

    public class FiveRepository : IFiveRepository
    {
        private readonly IConfiguration _config;
        private readonly IDbConnection _dbConnection;

        public FiveRepository(IConfiguration config)
        {
            _config = config;
            _dbConnection = new SqliteConnection(
                _config.GetSection("ConnectionStrings")
                    .GetSection("DefaultConnection").Value);
        }

        /// <summary>
        /// post method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Five Add(Five model)
        {
            string sql = "Insert into Fives(Id, Note) VALUE (2, 'Yera');Select Cast(Scope_Identity() as Int);";

            var id = _dbConnection.Query<int>(sql).Single();
            model.Id = id;
            return model;

        }

        public Five GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Five Update(Five model)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new System.NotImplementedException();
        }

        public List<Five> GetAll()
        {
            string sql = "Select * from Fives ORDER BY Id DESC";
            return _dbConnection.Query<Five>(sql).ToList();
        }

        public List<Five> GetAllWithPaging(int pageIndex, int pageSize = 10)
        {
            throw new System.NotImplementedException();
        }
    }

    [Route("api/[controller]")]
    // [Route("api/fives")]
    public class FiveServiceController : Controller
    {
        private readonly IFiveRepository _repository;

        public FiveServiceController(IFiveRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var fives = _repository.GetAll();
                if (fives == null) return NotFound("Nothing in DB");

                return Ok(fives); // 200 ok 
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

        [HttpPost]
        [Produces("application/json", Type = typeof(Five))]
        [Consumes("application/json")]
        public IActionResult Post([FromBody] Five model)
        {
            try
            {
                if (model.Note == null || model.Note.Length < 1) ModelState.AddModelError("Note", "note is required");

                if (!ModelState.IsValid) return BadRequest(ModelState); //400 error

                var m = _repository.Add(model);

                return Ok(m);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}