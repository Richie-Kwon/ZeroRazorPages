using System;

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

    public interface IPointRepository
    {
        
    }

    public class PointRepository : IPointRepository
    {
        
    }
}