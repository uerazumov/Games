using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Lab2
{
    // В Никнейме допустимы только русские буквы верхнего и нижнего регистра
    public class NicknameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var S = value as String;
            if (S.Length < 5)
            {
                return new ValidationResult(false, "Никнейм слишком короткий!");
            }
            if (S.Length > 15)
            {
                return new ValidationResult(false, "Никнейм слишком длинный!");
            }
            for (int i = 0; i < S.Length; i++)
            {
                int j = 0;
                while(j < AcceptableSymbols.Count)
                {
                    if (S.ToLower()[i] == AcceptableSymbols[j])
                    {
                        j = AcceptableSymbols.Count;
                    }
                    j++;
                }
                if (j == AcceptableSymbols.Count)
                {
                    return new ValidationResult(false, "В никнейме могут содержаться только буквы русского алфавита!");
                }
            }
            return new ValidationResult(true, String.Empty);
        }
        List<Char> AcceptableSymbols = new List<char>
        {
            'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я'
        };
    }
}
