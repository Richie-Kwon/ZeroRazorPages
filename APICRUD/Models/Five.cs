using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace APICRUD.Models
{
    /// <summary>
    ///Model class 
    /// </summary>
    public class Five
    {
        public int Id { get; set; }
        
        [Required]
        public string Note { get; set; }
    }

    public interface IFiveRepository
    {
        Five Add(Five model); 
        Five GetById(int id);
        Five Update(Five model);
        void Remove(int id);
        List<Five> GetAll();
        List<Five> GetAllWithPaging(int pageIndex, int pageSize=10);

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
        
        public Five Add(Five model)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        public List<Five> GetAllWithPaging(int pageIndex, int pageSize = 10)
        {
            throw new System.NotImplementedException();
        }
    }
}                              