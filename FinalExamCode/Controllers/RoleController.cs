using FinalExamCode.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalExamCode.Controllers
{
    public class RoleController : Controller
    {
        private readonly BookManagementContext _context;

        public RoleController(BookManagementContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var roles = _context.Role.ToList();
            return View(roles);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Role model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                Console.WriteLine("Validation Errors:");
                foreach (var error in errors)
                {
                    Console.WriteLine(error); 
                }
                ViewBag.Errors = errors;
                return View(model);
            }
            if (ModelState.IsValid)
            {
                _context.Role.Add(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var role = _context.Role.Find(id);
            if (role == null) return NotFound();

            return View(role);
        }
        [HttpPost]
        public IActionResult Edit(Role model)
        {
            if (ModelState.IsValid)
            {
                _context.Role.Update(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var role = _context.Role.Find(id);
            if (role == null)
            {
                return NotFound();
            }
            _context.Role.Remove(role);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}
