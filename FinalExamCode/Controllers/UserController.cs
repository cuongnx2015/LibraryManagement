using FinalExamCode.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExamCode.Controllers
{
    public class UserController : Controller
    {
        private readonly BookManagementContext _context;
        public UserController(BookManagementContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var users = _context.User
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ToList();
            return View(users);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Roles = _context.Role.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Username = model.Username,
                    Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    UserRoles = new List<UserRole>()
                };
                foreach (var roleId in model.SelectedRoleIds)
                {
                    user.UserRoles.Add(new UserRole { RoleId = roleId });
                }
                _context.User.Add(user);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Roles = _context.Role.ToList();
            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _context.User
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role) 
                .FirstOrDefault(u => u.Id == id);

            if (user == null) return NotFound();
            var model = new UserViewModel
            {
                Id = user.Id,
                Username = user.Username,
                SelectedRoleIds = user.UserRoles.Select(ur => ur.RoleId).ToList() 
            };
            ViewBag.Roles = _context.Role.ToList(); 
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(UserViewModel model)
        {
            var user = _context.User
                .Include(u => u.UserRoles)
                .FirstOrDefault(u => u.Id == model.Id);
            if (user == null) return NotFound();
            if (string.IsNullOrEmpty(model.Password))
            {
                model.Password = user.Password; 
            }
            if (ModelState.IsValid)
            {
                user.Username = model.Username;

                if (!string.IsNullOrEmpty(model.Password))
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
                }
                user.UserRoles.Clear();
                foreach (var roleId in model.SelectedRoleIds)
                {
                    user.UserRoles.Add(new UserRole { RoleId = roleId });
                }
                _context.User.Update(user);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Roles = _context.Role.ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = _context.User
                .Include(u => u.UserRoles) 
                .FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();
            _context.UserRole.RemoveRange(user.UserRoles);
            _context.User.Remove(user);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
