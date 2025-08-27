using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PojistakNET.Helpers
{
    public static class PhoneHelper
    {

        // Formátování telefonního čísla (optické přidání mezer)
        public static string FormatPhoneNumber(string phoneNumber)
        {
            // Odstranění všech nečíselných znaků
            phoneNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());

            // Formátování pro české číslo s předvolbou
            if (phoneNumber.StartsWith("420") && phoneNumber.Length == 9)
            {
                return $"+420 {phoneNumber.Substring(3, 3)} {phoneNumber.Substring(6, 3)} {phoneNumber.Substring(9)}";
            }
            // Pokud je délka čísla větší nebo rovna 9, použijeme formát bez předvolby
            else if (phoneNumber.Length >= 9)
            {
                return $"{phoneNumber.Substring(0, 3)} {phoneNumber.Substring(3, 3)} {phoneNumber.Substring(6)}";
            }

            // Pokud formát neodpovídá, vrátíme číslo bez změny
            return phoneNumber;
        }
    }
}
