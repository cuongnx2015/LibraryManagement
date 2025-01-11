using FinalExamCode.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace FinalExamCode.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BookManagementContext _context;
        public HomeController(BookManagementContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public IActionResult Index()
        {
            var carouselItems = _context.Carousel
                                        .Where(c => c.IsActive)
                                        .ToList();
            return View(carouselItems);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
