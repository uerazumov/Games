using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OneQuestionFourAnswers
{
    /// <summary>
    /// Логика взаимодействия для NewRecordWindow.xaml
    /// </summary>
    public partial class NewRecordWindow : Window
    {
        public NewRecordWindow()
        {
            InitializeComponent();
            Loaded += (obj, args) =>
            {
                UsernameTextBox.Focus();
                UsernameTextBox.SelectAll();
            };
        }
        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
