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
            AnswerIsSelect(null);
        }

        private void OnTime10Sec()
        {
            _window?.PlaySound(MainWindow.SoundType.Ends10SecSound);
            _vm.Timeout -= OnTime10Sec;
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            var close = MainWindowViewModel.OpenDialogWindow(MainWindowViewModel.DialogWindowType.ExitTheGame) ?? true;
            if (!close)
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
            _window = App.Current.MainWindow as MainWindow;
            GlobalLogger.Instance.Info("Была открыта страница Игры");
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
            MainWindowViewModel.OpenDialogWindow(MainWindowViewModel.DialogWindowType.StatisticWindow);
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
            var button = (StrechableButton) sender;
            var index = Convert.ToInt16(button.Tag);
            AnswerIsSelect(index);
        }

        private void AnswerIsSelect(Int16? index)
        {
            _vm.AnswerIsSelect(index);
            Hints.IsEnabled = false;
            var counter = 0;
            var timer = new Timer(1.3 * 1000) { AutoReset = true };
            timer.Elapsed += (obj, args) =>
            {
                Dispatcher.Invoke(() =>
                {
                    counter++;
                    if (counter == 1)
                    {
                        _vm.PaintTrueAnswer();
                    }
                    if (counter == 2)
                    {
                        CheckAnswer(index);
                        Hints.IsEnabled = true;
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
                    {
                        _window?.PlaySound(MainWindow.SoundType.CorrectAnswer);
                    }
                    break;
                case MainWindowViewModel.ResultType.Incorrect:
                    {
                        _window?.PlaySound(MainWindow.SoundType.LifeIsBroken);
                    }
                    break;
                case MainWindowViewModel.ResultType.Defeat:
                    {
                        _window?.PlaySound(MainWindow.SoundType.DefeatSound);
                        var close = MainWindowViewModel.OpenDialogWindow(MainWindowViewModel.DialogWindowType.DefeatWindow) ?? true;
                        if (!close)
                           {
                                 NavigationService ns = NavigationService.GetNavigationService(this);
                                 ns?.Navigate(new Uri("MainMenuPage.xaml", UriKind.Relative));
                           }
                       else
                           {
                            NavigationService.Refresh();
                           }
                    }
                    break;
                case MainWindowViewModel.ResultType.IncorrectNewRecord:
                {
                    _window?.PlaySound(MainWindow.SoundType.WinSound);
                        MainWindowViewModel.OpenDialogWindow(MainWindowViewModel.DialogWindowType.NewRecordWindow);
                    NavigationService ns = NavigationService.GetNavigationService(this);
                    ns?.Navigate(new Uri("MainMenuPage.xaml", UriKind.Relative));
                }
                break;
            }
        }
    }
}