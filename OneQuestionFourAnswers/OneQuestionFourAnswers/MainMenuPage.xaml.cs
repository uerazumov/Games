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
        //REVIEW: В команду
        private void ButtonClickStartGame(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns?.Navigate(new Uri("GamePage.xaml", UriKind.Relative));
        }
        //REVIEW:В команду
        private void ButtonClickInformation(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns?.Navigate(new Uri("InformationPage.xaml", UriKind.Relative));
        }
        //REVIEW:В команду
        private void ButtonClickLogOut(object sender, RoutedEventArgs e)
        {
            _vm.LogOut();
            _vm.ChangeSettings();
            RefreshInfo();
        }
        //REVIEW:Во вьюмодель
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
        //REVIEW:Во вьюмодель и через биндинг связать
        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            LogOutButton.ControlButton.Click += ButtonClickLogOut;
            RefreshInfo();
            InformationButton.ControlButton.Click += ButtonClickInformation;
            GlobalLogger.Instance.Info("Была открыта страница Главное Меню");
        }
        //REVIEW: в команду
        private void HighscoreButton(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns?.Navigate(new Uri("HighscoreTablePage.xaml", UriKind.Relative));
        }
    }
}
