using System;
using System.Windows;
using System.Windows.Navigation;

namespace OneQuestionFourAnswers
{
    public partial class HighscoreTable
    {
        public HighscoreTable()
        {
            InitializeComponent();
            DataContext = this;
        }
        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns?.Navigate(new Uri("MainMenuPage.xaml", UriKind.Relative));
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            BackButton.ControlButton.Click += BackButtonClick;
        }
    }
}
