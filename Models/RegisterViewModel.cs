using System.ComponentModel.DataAnnotations;

namespace PojistakNET.Models;

public class RegisterViewModel
{
    public string? Role { get; set; }

    [Required(ErrorMessage = "Jméno je povinné.")]
    [StringLength(50, ErrorMessage = "Jméno musí mít maximálně 50 znaků.")]
    // [RegularExpression(@"^[\p{L} \-]+$", ErrorMessage = "Jméno může obsahovat pouze písmena.")]
    [Display(Name = "Jméno")]
    public string FirstName { get; set; } = "";


    [Required(ErrorMessage = "Příjmení je povinné.")]
    [StringLength(50, ErrorMessage = "Příjmení musí mít maximálně 50 znaků.")]
    // [RegularExpression(@"^[\p{L} \-]+$", ErrorMessage = "Příjmení může obsahovat pouze písmena.")]
    [Display(Name = "Příjmení")]
    public string LastName { get; set; } = "";

    [Required(ErrorMessage = "Ulice je povinná.")]
    [StringLength(100)]
    [Display(Name = "Ulice")]
    public string Street { get; set; } = "";

    [Required(ErrorMessage = "Město je povinné.")]
    [StringLength(100)]
    [Display(Name = "Město")]
    public string City { get; set; } = "";

    [Required(ErrorMessage = "Stát je povinný.")]
    [StringLength(100)]
    [Display(Name = "Stát")]
    public string Country { get; set; } = "";

    [Required(ErrorMessage = "PSČ je povinné.")]
    [RegularExpression(@"^\d{3}\s?\d{2}$", ErrorMessage = "Zadejte platné PSČ (např. 123 45).")]
    [Display(Name = "PSČ")]
    public string Postcode { get; set; } = "";

    [Required]
    [StringLength(100, ErrorMessage = "Uživatelské jméno musí mít mezi 3 a 100 znaky.", MinimumLength = 3)]
    [Display(Name = "Uživatelské jméno")]
    public string Username { get; set; } = "";


    [Required(ErrorMessage = "Heslo je povinné.")]
    [StringLength(100, ErrorMessage = "Heslo musí mít mezi 6 a 100 znaky.", MinimumLength = 6)]
    [Display(Name = "Heslo")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";


    [Required(ErrorMessage = "Potvrzení hesla je povinné.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Hesla se neshodují.")]
    [Display(Name = "Potvrdit heslo")]
    public string ConfirmPassword { get; set; } = "";


    [Required(ErrorMessage = "Email je povinný.")]
    [EmailAddress(ErrorMessage = "Neplatný formát emailu.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Datum narození je povinné.")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Datum narození")]
//  [Range(typeof(DateTime), "1900-01-01", "2100-12-31", ErrorMessage = "Datum narození musí být mezi 1. lednem 1900 a 31. prosincem 2100.")]
    public DateTime DateOfBirth { get; set; }


    [Required(ErrorMessage = "Telefonní číslo je povinné.")]
    [Phone(ErrorMessage = "Neplatný formát telefonního čísla.")]
    [RegularExpression(@"^\+?[0-9\s]+$", ErrorMessage = "Telefonní číslo může obsahovat pouze číslice a mezery.")]
    [Display(Name = "Telefonní číslo")]
    public string PhoneNumber { get; set; } = "";

//  [Range(typeof(bool), "true", "true", ErrorMessage = "Musíte souhlasit s podmínkami.")]
    [Display(Name = "Souhlasím s podmínkami")]
    public bool AcceptTerms { get; set; }
}
