using System;
using System.Timers;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using LoggingService;

namespace OneQuestionFourAnswers
{
    public partial class GamePage
    {

        public GamePage()
        {
            InitializeComponent();
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            AButton.Button.CommandParameter = AButton;
            BButton.Button.CommandParameter = BButton;
            CButton.Button.CommandParameter = CButton;
            DButton.Button.CommandParameter = DButton;
            TimeButton.ControlButton.Click += ButtonClickTime;
        }

        private void ButtonClickTime(object sender, RoutedEventArgs e)
        {
            var clockSpin = new DoubleAnimation
            {
                From = 0,
                To = 360,
                Duration = new Duration(TimeSpan.FromMilliseconds(1000))
            };
            Storyboard.SetTargetName(clockSpin, Clock.Name);
            Storyboard.SetTargetProperty(clockSpin,
            new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"));
            var clockSpinStoryboard = new Storyboard();
            clockSpinStoryboard.Children.Add(clockSpin);
            clockSpinStoryboard.Begin(Clock);
        }       
    }
}