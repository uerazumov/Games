using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OneQuestionFourAnswers
{
    public class DialogManager
    {
        public enum DialogWindowType
        {
            DefeatWindow,
            ExitTheGame,
            ExitWindow,
            NewRecordWindow,
            StatisticWindow
        }

        private static Window _dialogWindow;

        public static bool? OpenDialogWindow(DialogWindowType type)
        {
            if (_dialogWindow != null)
            {
                _dialogWindow.Close();
            }
            switch (type)
            {
                case DialogWindowType.DefeatWindow:
                    _dialogWindow = new DefeatWindow { Owner = App.Current.MainWindow };
                    break;
                case DialogWindowType.ExitTheGame:
                    _dialogWindow = new ExitTheGame { Owner = App.Current.MainWindow };
                    break;
                case DialogWindowType.ExitWindow:
                    _dialogWindow = new ExitWindow { Owner = App.Current.MainWindow };
                    break;
                case DialogWindowType.NewRecordWindow:
                    _dialogWindow = new NewRecordWindow { Owner = App.Current.MainWindow };
                    break;
                case DialogWindowType.StatisticWindow:
                    _dialogWindow = new StatisticsWindow { Owner = App.Current.MainWindow };
                    break;
            }
            _dialogWindow.ShowDialog();
            var result = _dialogWindow.DialogResult;
            return result;
        }
    }
}
