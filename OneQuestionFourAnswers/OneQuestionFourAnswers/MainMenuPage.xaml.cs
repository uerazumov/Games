using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Navigation;

namespace OneQuestionFourAnswers
{
    public partial class MainMenuPage
    {
        public MainMenuPage()
        {
            InitializeComponent();
        }

        private void ButtonClickStartGame(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns?.Navigate(new Uri("GamePage.xaml", UriKind.Relative));
        }
        private void ButtonClickInformation(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns?.Navigate(new Uri("InformationPage.xaml", UriKind.Relative));
        }
        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            InformationButton.ControlButton.Click += ButtonClickInformation;
        }

        private void HighscoreButton(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns?.Navigate(new Uri("HighscoreTablePage.xaml", UriKind.Relative));
        }
    }
}
