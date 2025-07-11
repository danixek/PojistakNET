using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public AdminController(UserManager<ApplicationUser> userManager,
                               RoleManager<IdentityRole> roleManager,
                               ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        [HttpPost("promote/{id}")]
        public async Task<IActionResult> PromoteToAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            await _userManager.AddToRoleAsync(user, "admin");
            _logger.LogInformation($"User {user.UserName} was promoted to admin by {User.Identity?.Name}");

            TempData["Message"] = $"Uživatel {user.UserName} povýšen na admina.";
            return RedirectToAction("Index");
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
    }
}
