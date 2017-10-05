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
            var s = value as string;
            if (s.Length < 4)
            {
                return new ValidationResult(false, "Имя слишком короткое!");
            }
            for (int i = 0; i < s.Length; i++)
            {
                int j = 0;
                while (j < AcceptableSymbols.Count)
                {
                    if (s.ToLower()[i] == AcceptableSymbols[j])
                    {
                        j = AcceptableSymbols.Count;
                    }
                    j++;
                }
                if (j == AcceptableSymbols.Count)
                {
                    return new ValidationResult(false, "В имени могут содержаться только буквы русского алфавита!");
                }
            }
            return new ValidationResult(true, String.Empty);
        }
        List<char> AcceptableSymbols = Enumerable.Range('а', 33).Select(Convert.ToChar).ToList();
    }
}
