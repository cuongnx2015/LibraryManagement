using FinalExamCode.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExamCode.Controllers
{
    public class BookController : Controller
    {
        private readonly BookManagementContext _context;
        public BookController(BookManagementContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var books = _context.Book.Include(b => b.Category).ToList();
            return View(books ?? new List<Book>());
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _context.BookCategory.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(string Title, string Author, int CategoryId, IFormFile CoverImage, IFormFile PdfFile)
        {
            var book = new Book
            {
                Title = Title,
                Author = Author,
                CategoryId = CategoryId,
                CoverUrl = "/images/default-cover.jpg",
                PdfUrl = null
            };
            if (CoverImage != null && CoverImage.Length > 0)
            {
                var fileName = Path.GetFileName(CoverImage.FileName);
                var filePath = Path.Combine("wwwroot", "images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    CoverImage.CopyTo(stream);
                }
                book.CoverUrl = "/images/" + fileName;
            }
            if (PdfFile != null && PdfFile.Length > 0)
            {
                var pdfFileName = Path.GetFileName(PdfFile.FileName);
                var pdfFilePath = Path.Combine("wwwroot", "uploads", "pdfs", pdfFileName);
                using (var stream = new FileStream(pdfFilePath, FileMode.Create))
                {
                    PdfFile.CopyTo(stream);
                }
                book.PdfUrl = "/uploads/pdfs/" + pdfFileName;
            }
            _context.Book.Add(book);
            _context.SaveChanges();

            return RedirectToAction("Index", "Book");
        }
        public IActionResult Edit(int id)
        {
            var book = _context.Book.Find(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewBag.Categories = _context.BookCategory.ToList(); 
            return View(book);
        }
        [HttpPost]
        public IActionResult Edit(int id, string Title, string Author, int CategoryId, IFormFile CoverImage, IFormFile PdfFile)
        {
            var book = _context.Book.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }
            book.Title = Title;
            book.Author = Author;
            book.CategoryId = CategoryId;
            if (CoverImage != null && CoverImage.Length > 0)
            {
                if (!string.IsNullOrEmpty(book.CoverUrl))
                {
                    var oldCoverPath = Path.Combine("wwwroot", book.CoverUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldCoverPath))
                    {
                        System.IO.File.Delete(oldCoverPath);
                    }
                }
                var fileName = Path.GetFileName(CoverImage.FileName);
                var filePath = Path.Combine("wwwroot", "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    CoverImage.CopyTo(stream);
                }
                book.CoverUrl = "/images/" + fileName;
            }
            if (PdfFile != null && PdfFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(book.PdfUrl))
                {
                    var oldPdfPath = Path.Combine("wwwroot", book.PdfUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPdfPath))
                    {
                        System.IO.File.Delete(oldPdfPath);
                    }
                }
                var pdfFileName = Path.GetFileName(PdfFile.FileName);
                var pdfFilePath = Path.Combine("wwwroot", "uploads", "pdfs", pdfFileName);

                using (var stream = new FileStream(pdfFilePath, FileMode.Create))
                {
                    PdfFile.CopyTo(stream);
                }
                book.PdfUrl = "/uploads/pdfs/" + pdfFileName;
            }
            _context.SaveChanges();

            return RedirectToAction("Index", "Book");
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var book = _context.Book.FirstOrDefault(c => c.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(book.CoverUrl))
            {
                var filePath = Path.Combine("wwwroot", book.CoverUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            _context.Book.Remove(book);
            _context.SaveChanges();
            return RedirectToAction("Index", "Book");
        }
    }
}
