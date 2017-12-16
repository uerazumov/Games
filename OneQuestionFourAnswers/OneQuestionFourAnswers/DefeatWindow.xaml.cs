using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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
            (Owner as MainWindow)?.PlaySound(MainWindow.SoundType.NewGameSound);
            DialogResult = true;
            Close();
        }
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        private void SaveStatisticButtonClick(object sender, RoutedEventArgs e)
        {
            SaveStatistic.IsEnabled = false;
            SaveStatistic.Foreground = Brushes.Gray;
        }
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = ((FrameworkElement)Resources["KinectCursor"]).Cursor;
        }
    }
}
