using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PojistakNET.Models;

namespace PojistakNET.Controllers
{
    [Authorize(Roles = "superadmin")]
    public class AdminController : Controller

    /// Superadmin může spravovat všechny uživatele (včetně adminů) a role v systému.
    /// Má k dispozici i logování - kontrola administrativních akcí.
    /// A také může přidělovat role, mazat, registrovat
    /// a editovat uživatele či dokonce přidávat nové články.
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AdminController> _logger;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<ApplicationUser> userManager,
                               RoleManager<IdentityRole> roleManager,
                               ILogger<AdminController> logger,
                               ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [ActionName("Index")]
        public async Task<IActionResult> ManageAdmins()
        {
            var admins = await _userManager.GetUsersInRoleAsync("admin");
            var superadmins = await _userManager.GetUsersInRoleAsync("superadmin");

            var combinedUsers = admins.Union(superadmins).ToList();

            var model = new List<AdminUserViewModel>();

            foreach (var user in combinedUsers)
            {
                var isSuperAdmin = await _userManager.IsInRoleAsync(user, "superadmin");
                var isAdmin = await _userManager.IsInRoleAsync(user, "admin") || isSuperAdmin;

                model.Add(new AdminUserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName!,
                    FullName = user.FullName!,
                    Email = user.Email!,
                    IsAdmin = isAdmin,
                    IsSuperAdmin = isSuperAdmin
                });
            }

            return View(model);

        }

        public async Task<IActionResult> ManageUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            var model = new List<AdminUserViewModel>();

            foreach (var user in users)
            {
                var userId = await _userManager.FindByIdAsync(user.Id);
                var isSuperAdmin = await _userManager.IsInRoleAsync(userId!, "superadmin");
                var isAdmin = await _userManager.IsInRoleAsync(userId!, "admin") || isSuperAdmin;

                model.Add(new AdminUserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName!,
                    FullName = user.FullName!,
                    Email = user.Email!,
                    IsAdmin = isAdmin,
                    IsSuperAdmin = isSuperAdmin
                });
            }

            return View(model);
        }

        // public IActionResult ManageArticles()
        // {
        //    var articles = _context.Articles.ToList();
        //    return View(articles);
        // }

        [HttpPost("promote/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PromoteToAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            await _userManager.AddToRoleAsync(user, "admin");
            _logger.LogInformation($"User {user.UserName} was promoted to admin by {User.Identity?.Name}");

            TempData["Message"] = $"Uživatel {user.UserName} povýšen na admina.";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> DemoteFromAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            await _userManager.RemoveFromRoleAsync(user, "admin");

            _logger.LogInformation($"User {user.UserName} was demoted from admin by {User.Identity?.Name}");

            TempData["Message"] = $"Admin role odebrána uživateli {user.UserName}.";
            return RedirectToAction("ManageAdmins");
        }


        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            await _userManager.DeleteAsync(user);
            _logger.LogWarning($"User {user.UserName} was deleted by {User.Identity?.Name}");

            TempData["Message"] = $"Uživatel {user.UserName} byl odstraněn.";
            return RedirectToAction("Index");
        }

        // atd. – zablokování, přidání role, výpis logu...

        public async Task<IActionResult> Logs()
        {
            var logs = await _context.LogEntries.OrderByDescending(l => l.Timestamp).Take(100).ToListAsync();
            return View(logs);
        }
    }
}
