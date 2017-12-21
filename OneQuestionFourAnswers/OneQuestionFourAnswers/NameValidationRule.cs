using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using LoggingService;
using System.Text.RegularExpressions;

namespace OneQuestionFourAnswers
{
    public class NameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var regex = new Regex("^([А-Яа-я]{3,14})$");
            if (!regex.IsMatch(value as string))
            {
                GlobalLogger.Instance.Info("Неправильный формат имени");
                return new ValidationResult(false, "Неправильный формат имени");
            }
            return new ValidationResult(true, String.Empty);
        }

        private readonly List<char> _acceptableSymbols = Enumerable.Range('а', 33).Select(Convert.ToChar).ToList();
    }
}
