using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PojistakNET.Models; // Import modelů, např. LoginViewModel

namespace PojistakNET.Controllers;

public class AccountController : Controller
{
    // Zde se definují metody pro správu účtů
    // například Register, Login, Logout atd.

    // Metody pro správu účtů
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;

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
            return RedirectToAction("Index", "Insurer"); // nebo kam chceš
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

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            // Přidělení role "user" nově registrovanému uživateli
            await _userManager.AddToRoleAsync(user, "user");

            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View(model);
    }
}
