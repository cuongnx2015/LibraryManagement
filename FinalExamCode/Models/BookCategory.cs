using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExamCode.Models
{
    public class BookCategory
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AvatarUrl { get; set; }
        public ICollection<Book> Book { get; set; }
        [NotMapped] 
        public int BookCount => Book?.Count ?? 0; 
    }
}