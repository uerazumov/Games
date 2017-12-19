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

        //REVIEW:В команду
        private void YesButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        //REVIEW:В команду
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = ((FrameworkElement)Resources["KinectCursor"]).Cursor;
        }
    }
}
