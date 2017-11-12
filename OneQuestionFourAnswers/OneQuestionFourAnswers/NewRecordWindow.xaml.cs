using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace OneQuestionFourAnswers
{
    public partial class NewRecordWindow
    {
        private TextBox _textBox;

        public NewRecordWindow()
        {
            InitializeComponent();
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
    }
}
