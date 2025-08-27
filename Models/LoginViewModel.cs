using System;
using System.ComponentModel.DataAnnotations;

namespace PojistakNET.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Uživatelské jméno je povinné")]
    [Display(Name = "Uživatelské jméno")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Heslo je povinné")]
    [DataType(DataType.Password)]
    [Display(Name = "Heslo")]
    public required string Password { get; set; }

    [Display(Name = "Zapamatovat si mě")]
    public bool RememberMe { get; set; } = false;

    // Přidat další vlastnosti podle potřeby, např. pro 2FA
}
