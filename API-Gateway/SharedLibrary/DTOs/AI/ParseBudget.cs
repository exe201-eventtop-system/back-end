using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.DTOs.AI
{
    public static class ParseBudget
    {
        public static decimal ParseBudgetExtention(string? rawBudget)
        {
            if (string.IsNullOrWhiteSpace(rawBudget)) return 0;

            var cleaned = new string(rawBudget
                .Where(c => char.IsDigit(c) || c == ',' || c == '.')
                .ToArray());
            cleaned = cleaned.Replace(".", "").Replace(",", ".");

            return decimal.TryParse(cleaned, NumberStyles.Any, CultureInfo.InvariantCulture, out var result)
                ? result
                : 0;
        }
    }
}
