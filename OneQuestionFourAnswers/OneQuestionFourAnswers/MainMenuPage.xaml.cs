using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace OneQuestionFourAnswers
{
    /// <summary>
    /// Логика взаимодействия для MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : Page
    {
        public MainMenuPage()
        {
            InitializeComponent();
        }

        private void ButtonClickStartGame(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new Uri("GamePage.xaml", UriKind.Relative));
        }
        private void ButtonClickInformation(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new Uri("InformationPage.xaml", UriKind.Relative));
        }
        private void ButtonClickUpdate(object sender, RoutedEventArgs e)
        {
            (Window.GetWindow(this) as MainWindow).UpdateBar.Visibility = Visibility.Visible;
            StartBatton.IsEnabled = false;
            UpdateBatton.IsEnabled = false;
            var timer = new Timer(3 * 1000) { AutoReset = false };
            timer.Elapsed += (obj, args) =>
            {
                Dispatcher.Invoke(() =>
                {
                    (App.Current.MainWindow as MainWindow).UpdateBar.Visibility = Visibility.Collapsed;
                    StartBatton.IsEnabled = true;
                    UpdateBatton.IsEnabled = true;
                });
            };
            timer.Start();
        }
        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            InformationButton.ControlButton.Click += ButtonClickInformation;
        }

        private void HighscoreButton(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new Uri("HighscoreTablePage.xaml", UriKind.Relative));
        }
    }
}
