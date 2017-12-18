using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Navigation;
using LoggingService;

namespace OneQuestionFourAnswers
{
    public partial class MainMenuPage
    {
        private readonly MainWindowViewModel _vm;

        public MainMenuPage()
        {
            _vm = (MainWindowViewModel)Application.Current.Resources["MainWindowVM"];
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
            LogOutButton.DisableButton = _vm.GetLogInStatus();
            InformationButton.ControlButton.Click += ButtonClickInformation;
            GlobalLogger.Instance.Info("Была открыта страница Главное Меню");
        }

        private void HighscoreButton(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns?.Navigate(new Uri("HighscoreTablePage.xaml", UriKind.Relative));
        }
    }
}
