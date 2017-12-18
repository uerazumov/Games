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

        private void ButtonClickLogOut(object sender, RoutedEventArgs e)
        {
            _vm.LogOut();
            _vm.ChangeSettings();
            RefreshInfo();
        }

        private void RefreshInfo()
        {
            LogOutButton.DisableButton = _vm.GetLogInStatus();
            if (LogOutButton.DisableButton)
            {
                UserName.Visibility = Visibility.Collapsed;
            }
            else
            {
                UserName.Visibility = Visibility.Visible;
            }
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            LogOutButton.ControlButton.Click += ButtonClickLogOut;
            RefreshInfo();
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
