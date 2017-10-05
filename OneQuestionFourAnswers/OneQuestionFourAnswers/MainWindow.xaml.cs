using System.Windows;

namespace OneQuestionFourAnswers
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonClickSound(object sender, RoutedEventArgs e)
        {
            SoundButton.DisableButton = !SoundButton.DisableButton;
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            SoundButton.ControlButton.Click += ButtonClickSound;
        }
    }
}
