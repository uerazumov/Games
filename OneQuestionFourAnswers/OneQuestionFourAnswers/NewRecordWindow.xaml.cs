using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace OneQuestionFourAnswers
{
    public partial class NewRecordWindow
    {
        private TextBox _textBox;
        private readonly MainWindowViewModel _vm;

        public NewRecordWindow()
        {
            InitializeComponent();
            _vm = (MainWindowViewModel)Application.Current.Resources["MainWindowVM"];
            Loaded += (obj, args) =>
            {
                UsernameTextBox.Focus();
                UsernameTextBox.SelectAll();
                _textBox = BorderUsernameTextBox.Child as TextBox;
                Debug.Assert(_textBox != null, nameof(_textBox) + " != null");
                (_textBox).GotFocus += OnTextBoxFocused;
            };
        }
        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            if (!Validation.GetHasError(_textBox)&&((_textBox).Text != ""))
            {
                DialogResult = true;
                Close();
            }
            else
            {
                NoticeTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void OnTextBoxFocused(object sender, RoutedEventArgs e)
        {
            if (!Validation.GetHasError(_textBox))
            {
                return;
            }
            (_textBox).Text = "";
            var property = TextBox.TextProperty;
            Debug.Assert(property != null, "TextBox.TextProperty != null");
            Validation.ClearInvalid(_textBox.GetBindingExpression(property));
            NoticeTextBlock.Visibility = Visibility.Collapsed;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void SaveStatisticButtonClick(object sender, RoutedEventArgs e)
        {
            var result = _vm.CreateReport();
            if (result == true)
            {
                SaveStatistic.IsEnabled = false;
                SaveStatistic.Foreground = Brushes.Gray;
                ReportError.Visibility = Visibility.Collapsed;
                ReportSuccessfully.Visibility = Visibility.Visible;
            }
            else if (result == false)
            {
                ReportError.Visibility = Visibility.Visible;
            }
        }

        private void SaveRecordIntoVKButtonClick(object sender, RoutedEventArgs e)
        {
            bool? webResult = true;
            if (!_vm.IsTokenExist())
            {
                WebBrowser web = new WebBrowser();
                web.ShowDialog();
                web.Owner = Owner;
                webResult = web.DialogResult;
            }
            if (webResult != null)
            {
                if ((webResult ?? false) && _vm.CreateRec())
                {
                    SaveRecordIntoVK.IsEnabled = false;
                    SaveRecordIntoVK.Foreground = Brushes.Gray;
                    VkError.Visibility = Visibility.Collapsed;
                    VkSuccessfully.Visibility = Visibility.Visible;
                }
                else
                {
                    VkError.Visibility = Visibility.Visible;
                }
            }
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = ((FrameworkElement)Resources["KinectCursor"]).Cursor;
        }
    }
}
