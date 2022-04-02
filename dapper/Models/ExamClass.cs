using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace dapper.Models
{
    public class ExamClass
    {
    }

    /// <summary>
    /// model class
    /// </summary>
    public class Question
    {
        public int Id { get; set; }

        [Required] public string Title { get; set; }
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
}