using System.Windows;
using System.Windows.Input;

namespace OneQuestionFourAnswers
{
    public partial class StatisticsWindow
    {
        public StatisticsWindow()
        {
            InitializeComponent();
        }
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = ((FrameworkElement)Resources["KinectCursor"]).Cursor;
        }
    }
}
