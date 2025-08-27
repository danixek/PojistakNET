using PojistakNET.Models;
using System.ComponentModel.DataAnnotations;

namespace PojistakNET.Models
{
    public class Insurance
    {
        public int Id { get; set; }

        [Display(Name = "Pojištění")]
        [Required(ErrorMessage = "Název pojištění je vyžadován")]
        [StringLength(30, ErrorMessage = "Název pojištění nesmí být delší než {0} znaků")]
        public required string Name { get; set; } // Název pojištění

        [Display(Name = "Částka")]
        [Required(ErrorMessage = "Částka je vyžadována")]
        [Range(0, 999999999999.99, ErrorMessage = "Částka musí být mezi 0 a 999 999 999 999,99 znaků")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Cost { get; set; } // Částka pojištění

        [Display(Name = "Předmět pojištění")]
        [Required(ErrorMessage = "Předmět pojištění je vyžadován")]
        public required string ObjectOfInsurance { get; set; } // Předmět pojištění

        [Display(Name = "Platnost od")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Platnost od je vyžadována")]
        public DateTime ValidFrom { get; set; }

        [Display(Name = "Platnost do")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Platnost do je vyžadována")]
        public DateTime ValidUntil { get; set; }

        // Spárování s tabulkou - seznamem - pojištěnců
        public int InsurerId { get; set; }
        public Insurer? Insurer { get; set; } // Navigační vlastnost pro vztah
    }
}
