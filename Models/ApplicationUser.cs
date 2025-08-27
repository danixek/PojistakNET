using Microsoft.AspNetCore.Identity;
using System;

namespace PojistakNET.Models
{
    /// <summary>
    /// Rozšířený uživatelský model pro Evidence pojištění.
    /// Obsahuje základní identity vlastnosti i specifické údaje,
    /// které jsou důležité pro naši doménu.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Celé jméno uživatele.
        /// Používáme pro personalizaci komunikace, tisk smluv a reportů.
        /// </summary>
        public required string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Adresa uživatele pro doručení dokumentů či faktur.
        /// Může být rozšířeno o více polí (ulice, město, PSČ) podle potřeby.
        /// </summary>
        public required string Address { get; set; }

        /// <summary>
        /// Datum narození uživatele.
        /// Může být důležité pro věkové limity či ověření.
        /// </summary>
        public DateTime? DateOfBirth { get; set; }



        // Další vlastnosti účtu...
        public required string FirstName { get; set; }  = string.Empty;
        public required string LastName { get; set; }  = string.Empty;
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Postcode { get; set; }
    }
}
