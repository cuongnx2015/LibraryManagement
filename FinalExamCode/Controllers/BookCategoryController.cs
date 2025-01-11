using FinalExamCode.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExamCode.Controllers
{
    public class BookCategoryController : Controller
    {
        private readonly BookManagementContext _context;
        public BookCategoryController(BookManagementContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var categoriesWithBookCount = _context.BookCategory
                .Include(c => c.Book) 
                .ToList();
            return View(categoriesWithBookCount); 
        }
        public IActionResult BooksByCategory(int id)
        {
            var category = _context.BookCategory
                .Include(c => c.Book) 
                .FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound(); 
            }
            var bookCount = category.Book.Count();
            ViewBag.BookCount = bookCount;
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(int id, BookCategory model, IFormFile Avatar)
        {
            var category = _context.BookCategory.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            category.Title = model.Title;
            if (Avatar != null && Avatar.Length > 0)
            {
                var fileName = Path.GetFileName(Avatar.FileName);
                var filePath = Path.Combine("wwwroot/images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Avatar.CopyTo(stream);
                }
                category.AvatarUrl = "/images/" + fileName;
            }
            _context.SaveChanges();
            return RedirectToAction("Category","Admin");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.BookCategory.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category); 
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var category = _context.BookCategory.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(category.AvatarUrl))
            {
                var filePath = Path.Combine("wwwroot", category.AvatarUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            _context.BookCategory.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Category", "Admin");
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(); 
        }
        [HttpPost]
        public IActionResult Create(string Title, IFormFile Avatar)
        {
            if (ModelState.IsValid)
            {
                var maxId = _context.BookCategory.Max(c => (int?)c.Id) ?? 0; 
                var newId = maxId + 1; 
                var category = new BookCategory
                {
                    Id = newId,
                    Title = Title,
                    AvatarUrl = "/images/default-avatar.png" 
                };
                if (Avatar != null && Avatar.Length > 0)
                {
                    var fileName = Path.GetFileName(Avatar.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        Avatar.CopyTo(stream);
                    }
                    category.AvatarUrl = "/images/" + fileName;
                }
                _context.BookCategory.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Category", "Admin");
            }
            return View();
        }
    }
}
