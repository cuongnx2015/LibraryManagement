using FinalExamCode.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalExamCode.Controllers
{
    public class CarouselController : Controller
    {
        private readonly BookManagementContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public CarouselController(BookManagementContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View(_context.Carousel.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Carousel model, IFormFile ImagePath)
        {
            if (ImagePath != null && ImagePath.Length > 0)
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "images", ImagePath.FileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    ImagePath.CopyTo(stream);
                }
                model.ImagePath = "/images/" + ImagePath.FileName;
            }
            else
            {
                ModelState.AddModelError("ImagePath", "Image is required.");
                return View(model);
            }
            _context.Carousel.Add(model);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            var carousel = _context.Carousel.FirstOrDefault(c => c.Id == id);
            if (carousel == null)
            {
                return NotFound();
            }
            return View(carousel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string Title, string Description, bool IsActive, IFormFile ImagePath)
        {
            var carousel = _context.Carousel.FirstOrDefault(c => c.Id == id);
            if (carousel == null)
            {
                return NotFound();
            }
            carousel.Title = Title;
            carousel.Description = Description;
            carousel.IsActive = IsActive;
            if (ImagePath != null && ImagePath.Length > 0)
            {
                if (!string.IsNullOrEmpty(carousel.ImagePath))
                {
                    var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, carousel.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                var fileName = Path.GetFileName(ImagePath.FileName);
                var filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImagePath.CopyTo(stream);
                }
                carousel.ImagePath = "/images/" + fileName;
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Carousel");
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var carousel = _context.Carousel.FirstOrDefault(c => c.Id == id);
            if (carousel != null)
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "images", carousel.ImagePath.Split("/images/")[1]);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                _context.Carousel.Remove(carousel);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
