using PojistakNET.Models;
using System.ComponentModel.DataAnnotations;

namespace PojistakNET.Models
{
    public class Insurer
    {
        public int Id { get; set; }

        [Display(Name = "Jméno")]
        [Required(ErrorMessage = "Jméno je vyžadováno")]
        [StringLength(50, ErrorMessage = "Jméno nesmí být delší než {0} znaků")]
        public required string FirstName { get; set; }

        [Display(Name = "Příjmení")]
        [Required(ErrorMessage = "Příjmení je vyžadováno")]
        [StringLength(50, ErrorMessage = "Příjmení nesmí být delší než {0} znaků")]
        public required string LastName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email je vyžadován")]
        [StringLength(254, ErrorMessage = "Email nesmí být delší než {0} znaků.")]
        [EmailAddress(ErrorMessage = "Neplatný formát e-mailové adresy.")]
        public required string Email { get; set; }

        [Display(Name = "Telefon")]
        [Required(ErrorMessage = "Telefon je vyžadován")]
        [StringLength(15, ErrorMessage = "Telefon nesmí být delší než {0} znaků")]
        [RegularExpression(@"^\+?\d{1,4}(\s?\d{3}){2,4}$", ErrorMessage = "Neplatný formát telefonního čísla")]
        public required string Phone { get; set; }

        [Display(Name = "Ulice a číslo popisné")]
        [Required(ErrorMessage = "Ulice je vyžadována")]
        [StringLength(50, ErrorMessage = "Ulice nesmí být delší než {0} znaků")]
        public required string Street { get; set; }

        [Display(Name = "Město")]
        [Required(ErrorMessage = "Město je vyžadováno")]
        [StringLength(50, ErrorMessage = "Město nesmí být delší než {0} znaků")]
        public required string City { get; set; }

        [Display(Name = "PSČ")]
        [Required(ErrorMessage = "PSČ je vyžadováno")]
        [StringLength(10, ErrorMessage = "PSČ nesmí být delší než {0} znaků")]
        public required string Postcode { get; set; }

        // Navigační vlastnost - kolekce pojištění pro tohoto pojištěnce
        public ICollection<Insurance>? Insurances { get; set; }
    }
}

