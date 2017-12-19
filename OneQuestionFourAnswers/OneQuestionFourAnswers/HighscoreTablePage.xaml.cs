using System;
using System.Windows;
using System.Windows.Navigation;
using LoggingService;

namespace OneQuestionFourAnswers
{
    public partial class HighscoreTable
    {
        public HighscoreTable()
        {
            InitializeComponent();
            DataContext = this;
        }
        //REVIEW:В команду
        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns?.Navigate(new Uri("MainMenuPage.xaml", UriKind.Relative));
        }
        //REVIEW:В команду
        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            BackButton.ControlButton.Click += BackButtonClick;
            GlobalLogger.Instance.Info("Была открыта страница Таблица Рекордов");
        }
    }
}
