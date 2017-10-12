using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace OneQuestionFourAnswers
{
    public partial class GamePage
    {
        private readonly MainWindowViewModel _vm;
        private MainWindow _window;

        public GamePage()
        {
            InitializeComponent();
            _vm = (MainWindowViewModel) Application.Current.Resources["MainWindowVM"];
            _vm.Timeout += OnTimeout;
            _vm.Time10Sec += OnTime10Sec;
        }

        private void OnTimeout()
        {
            CheckAnswer(null);
            _vm.Timeout -= OnTimeout;
        }

        private void OnTime10Sec()
        {
            _window?.PlaySound(MainWindow.SoundType.Ends10SecSound);
            _vm.Timeout -= OnTime10Sec;
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
            _window = Window.GetWindow(this) as MainWindow;
        }

        private void ButtonClickTwoAnswers(object sender, RoutedEventArgs e)
        {
            _window?.PlaySound(MainWindow.SoundType.TwoAnswersSound);
            TwoAnswersButton.DisableButton = !TwoAnswersButton.DisableButton;
            TwoAnswersButton.IsEnabled = false;
        }

        private void ButtonClickStatistics(object sender, RoutedEventArgs e)
        {
            _window?.PlaySound(MainWindow.SoundType.StatisticSound);
            _vm?.UseHintStatistics();
            StatisticsButton.DisableButton = !StatisticsButton.DisableButton;
            StatisticsButton.IsEnabled = false;
            var sw = new StatisticsWindow {Owner = Window.GetWindow(this)};
            sw.ShowDialog();
        }

        private void ButtonClickTime(object sender, RoutedEventArgs e)
        {
            _window?.PlaySound(MainWindow.SoundType.TimeAddedSound);
            TimeButton.DisableButton = !TimeButton.DisableButton;
            TimeButton.IsEnabled = false;
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

        private void AnswerButtonClick(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            var index = Convert.ToInt16(button.Tag);
            _vm.AnswerIsSelect(index);
            _vm.DoStopTimerCommand.Execute(null);
            IsEnabled = false;
            var counter = 0;
            var timer = new Timer(1 * 1000) { AutoReset = true };
            timer.Elapsed += (obj, args) =>
            {
                counter++;
                Dispatcher.Invoke(() =>
                {
                    if (counter == 1)
                    {
                        _vm.PaintTrueAnswer();
                    }
                    if (counter == 2)
                    {
                        CheckAnswer(index);
                        IsEnabled = true;
                        timer.Stop();
                    }
                });
            };
            timer.Start();
        }

        private void CheckAnswer(int? index)
        {

            switch (_vm.IsCorrectAnswer(index))
            {
                case MainWindowViewModel.ResultType.Correct:
                    _vm.StartNewRound();
                    break;
                case MainWindowViewModel.ResultType.Incorrect:
                {
                    _window?.PlaySound(MainWindow.SoundType.DefeatSound);
                    var dfw = new DefeatWindow {Owner = Window.GetWindow(this)};
                    var close = dfw.ShowDialog() ?? true;
                    if (!close)
                    {
                        NavigationService ns = NavigationService.GetNavigationService(this);
                        ns?.Navigate(new Uri("MainMenuPage.xaml", UriKind.Relative));
                    }
                    else
                    {
                        _vm.DoOpenNewGameCommand.Execute(null);
                        NavigationService?.Refresh();
                    }
                }
                    break;
                case MainWindowViewModel.ResultType.IncorrectNewRecord:
                {
                    _window?.PlaySound(MainWindow.SoundType.WinSound);
                    var nrw = new NewRecordWindow {Owner = Window.GetWindow(this)};
                    var close = nrw.ShowDialog() ?? false;
                    if (close)
                    {
                        NavigationService ns = NavigationService.GetNavigationService(this);
                        ns?.Navigate(new Uri("MainMenuPage.xaml", UriKind.Relative));
                    }
                }
                    break;
            }
        }
    }
}