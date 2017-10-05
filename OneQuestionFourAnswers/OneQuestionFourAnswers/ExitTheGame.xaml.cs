using System.Windows;

namespace OneQuestionFourAnswers
{
    public partial class ExitTheGame
    {
        public ExitTheGame()
        {
            InitializeComponent();
        }

        private void YesButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }      
    }
}
