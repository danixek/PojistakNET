using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PojistakNET.Models; // Import modelů, např. LoginViewModel
using PojistakNET.Services;

namespace PojistakNET.Controllers;

public class AccountController : Controller
{
    // Zde se definují metody pro správu účtů
    // například Register, Login, Logout atd.

    // Metody pro správu účtů
    private readonly ILogService _logService;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogService logService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _logService = logService;    // logger pro logování

    }

    // Přihlášení uživatele
    // Zobrazí přihlašovací formulář
    // GET: /Account/Login
    [HttpGet]
    public IActionResult Login() => View();

    // Přihlášení uživatele
    // Zpracuje přihlašovací formulář
    // POST: /Account/Login
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _signInManager.PasswordSignInAsync(
            model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            await _logService.LogAsync("Info", $"Uživatel {model.Username} úspěšně přihlášen.", model.Username);
            return RedirectToAction("Index", "Home"); // nebo kam chceš
        }
        else
        {
            ModelState.AddModelError("", "Neplatné přihlašovací údaje");
            return View(model);
        }
    }

    // Odhlášení uživatele
    // Zpracuje odhlášení uživatele
    // POST: /Account/Logout
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        await _logService.LogAsync("Info", $"Uživatel {User.Identity?.Name} se odhlásil.", User.Identity?.Name);
        return RedirectToAction("Index", "Home");
    }

    // Registrace nového uživatele
    // Zobrazí registrační formulář
    // GET: /Account/Register
    [HttpGet]
    public IActionResult Register() => View();

    // Registrace nového uživatele
    // Zpracuje registrační formulář
    // Stále chybí zaznamenávání již registrovaných uživatelů v databázi
    // uživatelé tak mohou omylem vytvářet duplicitní účty
    // POST: /Account/Register
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        var isAdminCreating = User.IsInRole("admin") || User.IsInRole("superadmin");
        
        if (!ModelState.IsValid)
            return View(model);

        var user = new ApplicationUser
        {
            UserName = model.Username,
            Email = model.Email,
            FullName = $"{model.FirstName} {model.LastName}",
            Address = $"{model.Street}, {model.City}, {model.Postcode}, {model.Country}", // nebo podle potřeby
            DateOfBirth = model.DateOfBirth, // pokud je v modelu
            FirstName = model.FirstName,
            LastName = model.LastName,
            Street = model.Street,
            City = model.City,
            Country = model.Country,
            Postcode = model.Postcode,
            PhoneNumber = model.PhoneNumber,
        };

        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            ModelState.AddModelError("", "Účet s tímto e-mailem již existuje.");
            
            return View(model);
        }
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {

            // Zkontroluj, zda je to první uživatel v systému
            var usersCount = await _userManager.Users.CountAsync();
            if (usersCount == 1)
            {
                // Zkontroluj, že role "superadmin" existuje
                if (!await _roleManager.RoleExistsAsync("superadmin"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("superadmin"));
                }

                // Přidej roli superadmin
                await _userManager.AddToRoleAsync(user, "superadmin");
                await _logService.LogAsync("Warning", $"Uživatel {model.Username} se úspěšně registroval, a byly mu přiděleny superadmin práva.", model.Username);

            }
            else
            {
                var roleToAssign = model.Role == "admin" ? "admin" : "user";
                
                // Zkontroluj, že role "user" existuje
                if (!await _roleManager.RoleExistsAsync(roleToAssign))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleToAssign));
                }
                
                // Přidělení role "user" (či "admin") nově registrovanému uživateli
                await _userManager.AddToRoleAsync(user, roleToAssign);
                if (!isAdminCreating)
                {
                    await _logService.LogAsync("Success",
                        $"Uživatel {model.Username} se úspěšně registroval. Počet uživatelů v DB: {usersCount}",
                        model.Username);
                }
                else
                {
                    await _logService.LogAsync("Warning",
                        $"Superadmin zaregistroval nový účet ({roleToAssign}) - {model.Username}. Počet uživatelů v DB: {usersCount}",
                        User.Identity?.Name);
                }
            }

            // Přihlášení uživatele po úspěšné registraci
            // V případě registraci superadminem k automatickému přihlášení nedojde
            if (!isAdminCreating)
            {
                // Uživatel se registruje sám – přihlásíme ho a přesměrujeme
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Superadmin registruje – nechceme redirect ani automatické přihlášení
                // Můžeš dát třeba ViewBag nebo TempData pro potvrzení úspěchu
                ViewBag.Message = "Uživatel úspěšně vytvořen administrátorem.";
                return RedirectToAction("Index", "Admin");
            }
        }

        return View(model);
    }
}
