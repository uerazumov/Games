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
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = ((FrameworkElement)Resources["KinectCursor"]).Cursor;
        }
    }
}
