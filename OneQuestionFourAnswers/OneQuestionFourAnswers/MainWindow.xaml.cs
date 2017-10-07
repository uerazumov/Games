using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;

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
            TwoAnswersSound
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

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            SoundButton.ControlButton.Click += ButtonClickSound;
            Main.NavigationService.Navigated += (obj, args) => { Main.NavigationService.RemoveBackEntry(); };
            _backgroundPlayer.Open(new Uri(@".\SoundsAndMusic\Backround.mp3", UriKind.Relative));
            _backgroundPlayer.Play();
            _backgroundPlayer.MediaEnded += (o, args) =>
            {
                _backgroundPlayer.Position = TimeSpan.Zero;
                _backgroundPlayer.Play();
            };
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
            }
            _soundPlayer.Open(new Uri(source, UriKind.Relative));
            if(!SoundButton.DisableButton)
            {
                _soundPlayer.Play();
            }
        }
    }
}