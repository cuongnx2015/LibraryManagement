using System;

namespace FinalExamCode.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public User()
        {
            UserRoles = new List<UserRole>();
        }
    }

}
