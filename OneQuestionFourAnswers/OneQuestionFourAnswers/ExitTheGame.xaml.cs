using System.Windows;
using System.Windows.Input;

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
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = ((FrameworkElement)Resources["KinectCursor"]).Cursor;
        }
    }
}
