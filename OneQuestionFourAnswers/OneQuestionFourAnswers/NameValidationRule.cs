using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace OneQuestionFourAnswers
{
    public class NameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var S = value as string;
            if (S.Length < 4)
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
                while (j < AcceptableSymbols.Count)
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
        List<char> AcceptableSymbols = Enumerable.Range('а', 33).Select(Convert.ToChar).ToList();
    }
}
