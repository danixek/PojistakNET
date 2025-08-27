using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace PojistakNET.Helpers
{
    public static class MoneyHelper
    {
        // Formátování částky s podporou tečky a čárky
        public static string FormatMoney(decimal cost)
        {
            // Pokud je částka nulová, vrací prázdný řetězec
            if (cost == 0)
                return "0,00 Kč";

            // Použití kultury pro formátování s desetinnou čárkou
            var culture = new CultureInfo("cs-CZ"); // Česká kultura
            return string.Format(culture, "{0:N2}", cost) + " Kč";  // Formátování na 2 desetinná místa
        }
    }
}
