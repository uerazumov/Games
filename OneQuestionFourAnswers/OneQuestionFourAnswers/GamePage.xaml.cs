using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace OneQuestionFourAnswers
{
    public partial class GamePage
    {
        public GamePage()
        {
            InitializeComponent();
        }
        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            var etg = new ExitTheGame {Owner = Window.GetWindow(this)};
            var close = etg.ShowDialog() ?? false;
                if (close)
                {
                    NavigationService ns = NavigationService.GetNavigationService(this);
                    ns?.Navigate(new Uri("MainMenuPage.xaml", UriKind.Relative));
                }
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            BackButton.ControlButton.Click += BackButtonClick;
            TimeButton.ControlButton.Click += ButtonClickTime;
            StatisticsButton.ControlButton.Click += ButtonClickStatistics;
            TwoAnswersButton.ControlButton.Click += ButtonClickTwoAnswers;
        }

        private void ButtonClickTwoAnswers(object sender, RoutedEventArgs e)
        {
            TwoAnswersButton.DisableButton = !TwoAnswersButton.DisableButton;
            TwoAnswersButton.IsEnabled = false;
        }
        private void ButtonClickStatistics(object sender, RoutedEventArgs e)
        {
            StatisticsButton.DisableButton = !StatisticsButton.DisableButton;
            StatisticsButton.IsEnabled = false;
            var sw = new StatisticsWindow {Owner = Window.GetWindow(this)};
            sw.ShowDialog();  
        }
        private void ButtonClickTime(object sender, RoutedEventArgs e)
        {
            TimeButton.DisableButton = !TimeButton.DisableButton;
            TimeButton.IsEnabled = false;
            var clockSpin = new DoubleAnimation
            {
                From = 0,
                To = 360,
                Duration = new Duration(TimeSpan.FromMilliseconds(1000))
            };
            Storyboard.SetTargetName(clockSpin, Clock.Name);
            Storyboard.SetTargetProperty(clockSpin, new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"));
            var clockSpinStoryboard = new Storyboard();
            clockSpinStoryboard.Children.Add(clockSpin);
            clockSpinStoryboard.Begin(Clock);
        }

        private void AnswerButtonClick(object sender, RoutedEventArgs e)
        {
            var nrw = new NewRecordWindow {Owner = Window.GetWindow(this)};
            var close = nrw.ShowDialog() ?? false;
            if (close)
            {
                NavigationService ns = NavigationService.GetNavigationService(this);
                ns?.Navigate(new Uri("MainMenuPage.xaml", UriKind.Relative));
            }

            //DefeatWindow dfw = new DefeatWindow();
            //dfw.Owner = Window.GetWindow(this);
            //bool close = (bool)dfw.ShowDialog();
            //if (close)
            //{
            //    NavigationService ns = NavigationService.GetNavigationService(this);
            //    ns.Navigate(new Uri("MainMenuPage.xaml", UriKind.Relative));
            //}
            //else
            //{
            //    NavigationService.Refresh();
            //}
        }
    }
}
