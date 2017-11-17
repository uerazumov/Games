using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace OneQuestionFourAnswers
{
    public partial class MainWindow
    {
        public enum SoundType
        {
            DefeatSound,
            WinSound,
            Ends10SecSound,
            StatisticSound,
            NewGameSound,
            TimeAddedSound,
            TwoAnswersSound,
            CorrectAnswer,
            LifeIsBroken
        }

        private readonly MediaPlayer _backgroundPlayer;
        private readonly MediaPlayer _soundPlayer;

        public MainWindow()
        {
            InitializeComponent();
            _backgroundPlayer = new MediaPlayer();
           _soundPlayer = new MediaPlayer() {Volume = 0.25};
        }

        private void ButtonClickSound(object sender, RoutedEventArgs e)
        {
            SoundButton.DisableButton = !SoundButton.DisableButton;
            if (SoundButton.DisableButton)
            {
                _backgroundPlayer.Pause();
                _soundPlayer.Stop();
            }
            else
            {
                _backgroundPlayer.Play();
            }
        }

        private void ButtonClickCommentator(object sender, RoutedEventArgs e)
        {
            CommentatorButton.DisableButton = !CommentatorButton.DisableButton;
            if (CommentatorButton.DisableButton)
            {
                _soundPlayer.Volume = 0;
            }
            else
            {
                _soundPlayer.Volume = 0.25;
            }
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = ((FrameworkElement)Resources["KinectCursor"]).Cursor;
            ExitButton.ControlButton.Click += Exit;
            SoundButton.ControlButton.Click += ButtonClickSound;
            CommentatorButton.ControlButton.Click += ButtonClickCommentator;
            Main.NavigationService.Navigated += (obj, args) => { Main.NavigationService.RemoveBackEntry(); };
            _backgroundPlayer.Open(new Uri(@".\SoundsAndMusic\Backround.mp3", UriKind.Relative));
            _backgroundPlayer.Play();
            _backgroundPlayer.MediaEnded += (o, args) =>
            {
                _backgroundPlayer.Position = TimeSpan.Zero;
                _backgroundPlayer.Play();
            };
            var _vm = (MainWindowViewModel)Application.Current.Resources["MainWindowVM"];
            _vm.ChangeWidthAndHeight((int)(App.Current.MainWindow as MainWindow).Width, (int)(App.Current.MainWindow as MainWindow).Height);
        }

        public void Exit(object sender, RoutedEventArgs e)
        {
            var ew = new ExitWindow { Owner = this };
            var close = ew.ShowDialog() ?? false;
            if (close)
            {
                Application.Current.Shutdown();
            }
        }

        public void PlaySound(SoundType type)
        {
            string source = null;
            switch (type)
            {
                case SoundType.DefeatSound:
                    source = @".\SoundsAndMusic\Defeat.mp3";
                    break;
                case SoundType.WinSound:
                    source = @".\SoundsAndMusic\Win.mp3";
                    break;
                case SoundType.Ends10SecSound:
                    source = @".\SoundsAndMusic\Ends10sec.mp3";
                    break;
                case SoundType.StatisticSound:
                    source = @".\SoundsAndMusic\Statistic.wav";
                    break;
                case SoundType.NewGameSound:
                    source = @".\SoundsAndMusic\NewGame.mp3";
                    break;
                case SoundType.TimeAddedSound:
                    source = @".\SoundsAndMusic\TimeAdded.wav";
                    break;
                case SoundType.TwoAnswersSound:
                    source = @".\SoundsAndMusic\TwoAnswers.mp3";
                    break;
                case SoundType.CorrectAnswer:
                    source = @".\SoundsAndMusic\CorrectAnswerSound.mp3";
                    break;
                case SoundType.LifeIsBroken:
                    source = @".\SoundsAndMusic\LifeIsBrokenSound.mp3";
                    break;
            }
            if (source != null) _soundPlayer.Open(new Uri(source, UriKind.Relative));
            if(!CommentatorButton.DisableButton)
            {
                _soundPlayer.Play();
            }
        }
    }
}