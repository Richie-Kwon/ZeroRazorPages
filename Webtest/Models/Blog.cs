using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webtest.Models
{
    [Table("Blogs")]
    public class Blog
    {
        public int Id  { get; set; }
        public string Name  { get; set; }
        public ICollection<Post> Posts { get; set; }
    }

    [Table("Posts")]
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Blog Blog { get; set; }
        public int BlogId { get; set; }
    }
}