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
        private void ButtonClickUpdate(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this) as MainWindow;
            Debug.Assert(window != null, nameof(window) + " != null");
            window.UpdateBar.Visibility = Visibility.Visible;
            StartBatton.IsEnabled = false;
            UpdateBatton.IsEnabled = false;
            // Временный код
            var timer = new Timer(3 * 1000) { AutoReset = false };
            timer.Elapsed += (obj, args) =>
            {
                Dispatcher.Invoke(() =>
                {
                    window.UpdateBar.Visibility = Visibility.Collapsed;
                    StartBatton.IsEnabled = true;
                    UpdateBatton.IsEnabled = true;
                });
            };
            timer.Start();
            // Конец временного кода
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
