using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using LoggingService;

namespace OneQuestionFourAnswers
{
    public class NameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var s = value as string;
            if (s == null)
            {
                GlobalLogger.Instance.Info("Пользователь не ввёл имя при попытке сохранить Рекорд");
                return new ValidationResult(false, "Имя не введено"); 
            }
            if (s.Length < 3)
            {
                GlobalLogger.Instance.Info("Пользователь ввёл слишком короткое имя при попытке сохранить Рекорд");
                return new ValidationResult(false, "Имя слишком короткое!");
            }
            if (s.Length > 15)
            {
                GlobalLogger.Instance.Info("Пользователь ввёл слишком длинное имя при попытке сохранить Рекорд");
                return new ValidationResult(false, "Имя слишком длинное!");
            }
            //REVIEW: Методы Any и Contains обнялись и заплакали. А вообще, такие вещи решаются регулярными выражениями
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
                    GlobalLogger.Instance.Debug("Пользователь ввёл имя не на русском языке при попытке сохранить Рекорд");
                    return new ValidationResult(false, "В имени могут содержаться только буквы русского алфавита!");
                }
            }
            return new ValidationResult(true, String.Empty);
        }

        private readonly List<char> _acceptableSymbols = Enumerable.Range('а', 33).Select(Convert.ToChar).ToList();
    }
}
