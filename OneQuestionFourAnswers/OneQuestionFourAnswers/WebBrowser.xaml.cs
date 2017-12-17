using System.Windows;
using LoggingService;
using System;

namespace OneQuestionFourAnswers
{
    public partial class WebBrowser : Window
    {
        private readonly MainWindowViewModel _vm;

        public WebBrowser()
        {
            InitializeComponent();
            _vm = (MainWindowViewModel)Application.Current.Resources["MainWindowVM"];
        }

        private void OnWebViewNavigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                var fix = new Uri(e.Uri.ToString().Replace("#", "?"));
                var query = fix.Query;
                var dict = System.Web.HttpUtility.ParseQueryString(query);
                if (Array.IndexOf(dict.AllKeys, "access_token") != -1)
                {
                    _vm.SaveToken(dict["access_token"], dict["user_id"]);
                    _vm.ChangeUserName();
                    Close();
                    GlobalLogger.Instance.Info("Получен токен пользователя");
                }
                if (Array.IndexOf(dict.AllKeys, "error") != -1)
                {
                    Close();
                    GlobalLogger.Instance.Info("Не удалось получить токен пользователя");
                }
            }
            catch
            {
                GlobalLogger.Instance.Error("Произошла ошибка при авторизации пользователя");
            }
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            WebView.Navigate(_vm.GetAuthUrl());
        }
    }
}
