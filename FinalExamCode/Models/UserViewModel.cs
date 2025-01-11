using System.ComponentModel.DataAnnotations;

namespace FinalExamCode.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? Password { get; set; }
        public List<int> SelectedRoleIds { get; set; } = new List<int>();
        public UserViewModel()
        {
            SelectedRoleIds = new List<int>();
        }
    }


}
