using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZeroRazorPages.Models
{
    [Table("RolesTypes")]
    public class RoleType
    {
        [Key] 
        public int Id { get; set; }
        
        [Display(Name = "Role Name")]
        public string Name { get; set; }

        public bool Active { get; set; }
    }
}