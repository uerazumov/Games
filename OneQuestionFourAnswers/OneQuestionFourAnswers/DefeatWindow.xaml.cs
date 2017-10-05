using System.Windows;

namespace OneQuestionFourAnswers
{
    public partial class DefeatWindow
    {
        public DefeatWindow()
        {
            InitializeComponent();
        }

        private void YesButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
