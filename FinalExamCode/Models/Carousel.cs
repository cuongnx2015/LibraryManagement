using System.ComponentModel.DataAnnotations;

namespace FinalExamCode.Models
{
    public class Carousel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; } 
        public bool IsActive { get; set; } = true; 
    }
}
