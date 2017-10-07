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
            (Owner as MainWindow)?.PlaySound(MainWindow.SoundType.NewGameSound);
            DialogResult = true;
            Close();
        }
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
