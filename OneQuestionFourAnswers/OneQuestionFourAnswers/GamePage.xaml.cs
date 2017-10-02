using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OneQuestionFourAnswers
{
    /// <summary>
    /// Логика взаимодействия для GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        public GamePage()
        {
            InitializeComponent();
        }
        private void BackButtonClick(object sender, RoutedEventArgs e)
        {     
                ExitTheGame etg = new ExitTheGame();
                etg.Owner = Window.GetWindow(this);
                var close = etg.ShowDialog() ?? false;
                if (close)
                {
                    NavigationService ns = NavigationService.GetNavigationService(this);
                    ns.Navigate(new Uri("MainMenuPage.xaml", UriKind.Relative));
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
            StatisticsWindow stw = new StatisticsWindow();
            stw.Owner = Window.GetWindow(this);
            stw.ShowDialog();
            StatisticsButton.DisableButton = !StatisticsButton.DisableButton;
            StatisticsButton.IsEnabled = false;
        }
        private void ButtonClickTime(object sender, RoutedEventArgs e)
        {
            TimeButton.DisableButton = !TimeButton.DisableButton;
            TimeButton.IsEnabled = false;
            
            DoubleAnimation ClockSpin = new DoubleAnimation();
            ClockSpin.From = 0;
            ClockSpin.To = 360;
            ClockSpin.Duration = new Duration(TimeSpan.FromMilliseconds(1000));
            Storyboard.SetTargetName(ClockSpin, Clock.Name);
            Storyboard.SetTargetProperty(ClockSpin, new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"));
            Storyboard ClockSpinStoryboard = new Storyboard();
            ClockSpinStoryboard.Children.Add(ClockSpin);
            ClockSpinStoryboard.Begin(Clock);
        }

        private void AnswerButtonClick(object sender, RoutedEventArgs e)
        {
            //NewRecordWindow nrw = new NewRecordWindow();
            //nrw.Owner = Window.GetWindow(this);
            //var close = nrw.ShowDialog() ?? false;
            //if (close)
            //{
            //    NavigationService ns = NavigationService.GetNavigationService(this);
            //    ns.Navigate(new Uri("MainMenuPage.xaml", UriKind.Relative));
            //}

            DefeatWindow dfw = new DefeatWindow();
            dfw.Owner = Window.GetWindow(this);
            bool close = (bool)dfw.ShowDialog();
            if (close)
            {
                NavigationService ns = NavigationService.GetNavigationService(this);
                ns.Navigate(new Uri("MainMenuPage.xaml", UriKind.Relative));
            }
            else
            {
                NavigationService.Refresh();
            }
        }
    }
}
