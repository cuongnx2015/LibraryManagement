using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExamCode.Models
{
        public class Role
        {
            public int Id { get; set; }
            [Required(ErrorMessage = "The Name field is required.")]
            public string Name { get; set; }
            public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        }
}