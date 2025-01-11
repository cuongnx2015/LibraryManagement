using FinalExamCode.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExamCode.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly BookManagementContext _context;
        public AdminController(BookManagementContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Category()
        {
            var categories = _context.BookCategory.ToList();
            return PartialView(categories); 
        }
        public IActionResult Books()
        {
            return PartialView(); 
        }
        public IActionResult Users()
        {
            return PartialView();
        }
    }
}
