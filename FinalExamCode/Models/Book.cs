namespace FinalExamCode.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? CoverUrl { get; set; } 
        public string? PdfUrl { get; set; }  
        public int CategoryId { get; set; } 
        public BookCategory Category { get; set; }
    }
}
