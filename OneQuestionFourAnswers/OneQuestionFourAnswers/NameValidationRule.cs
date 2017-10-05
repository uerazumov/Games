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
            if (s == null)
            {
                return new ValidationResult(false, "s is null");
            }
            if (s.Length < 4)
            {
                return new ValidationResult(false, "Имя слишком короткое!");
            }
            for (var i = 0; i < s.Length; i++)
            {
                var j = 0;
                while (j < _acceptableSymbols.Count)
                {
                    if (s.ToLower()[i] == _acceptableSymbols[j])
                    {
                        j = _acceptableSymbols.Count;
                    }
                    j++;
                }
                if (j == _acceptableSymbols.Count)
                {
                    return new ValidationResult(false, "В имени могут содержаться только буквы русского алфавита!");
                }
            }
            return new ValidationResult(true, String.Empty);
        }

        private readonly List<char> _acceptableSymbols = Enumerable.Range('а', 33).Select(Convert.ToChar).ToList();
    }
}
