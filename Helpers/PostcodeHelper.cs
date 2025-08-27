using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PojistakNET.Helpers
{
    public static class PostcodeHelper
    {
        // Formátování českého PSČ
        public static string FormatPostcode(string postcode)
        {
            // Odstranění všech nečíselných znaků
            postcode = new string(postcode.Where(char.IsDigit).ToArray());

            // Formátování pro české PSČ (5 číslic, mezeru po třetí číslici)
            if (postcode.Length == 5)
            {
                return $"{postcode.Substring(0, 3)} {postcode.Substring(3, 2)}";
            }

            // Pokud PSČ neodpovídá českému formátu, vrátí se původní hodnota
            return postcode;
        }
    }
}
