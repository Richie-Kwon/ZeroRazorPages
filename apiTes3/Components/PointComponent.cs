using System;
using Microsoft.AspNetCore.Mvc;

namespace apiTes3.Components
{
    public class PointComponent
    {
    }

    public class Point
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int TotalPoint { get; set; }
    }

    public class PointLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int NewPoint { get; set; }
        public DateTimeOffset Created { get; set; }
    }

    public class PointStatus
    {
        public int Gold { get; set; }
        public int Silver { get; set; }
        public int Bronze { get; set; }
        
    }

    public interface IPointRepository
    {
        int GetTotalPointByUserId(int userId = 123);
    }

    public class PointRepository : IPointRepository
    {
        public int GetTotalPointByUserId(int userId = 123)
        {
            return userId;
        }
    }

    public interface IPointLogRepository
    {
    }

    public class PointLogRepository : IPointLogRepository
    {
    }

    public class PointController : Controller
    {
        private readonly IPointRepository _repository;

        public PointController(IPointRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var myPoint = _repository.GetTotalPointByUserId();
            ViewBag.MyPoint = myPoint;
            return View();
        }
    }

    [Route("api/[controller]")]
    public class PointServiceController : Controller
    {
        private readonly IPointRepository _repository;

        public PointServiceController(IPointRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            var myPoint = _repository.GetTotalPointByUserId();
            var json = new { myPoint };
            return Ok(json);
        }

        [HttpGet]
        [Route("{userId:int}")]
        public IActionResult Get(int userId)
        {
            var myPoint = _repository.GetTotalPointByUserId(userId);
            var json = new { myPoint };
            return Ok(json);
        }
    }

    public class PointLogController : Controller
    {
    }

    [Route("api/[controller]")]
    public class PointLogServiceController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            var json = new { Point = 123 };
            return Ok(json);
        }
    }
}