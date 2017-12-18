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
                    _vm.ChangeSettings();
                    DialogResult = true;
                    WebView.Navigate("javascript:void((function(){var a,b,c,e,f;f=0;a=document.cookie.split('; ');for(e=0;e<a.length&&a[e];e++){f++;for(b='.'+location.host;b;b=b.replace(/^(?:%5C.|[^%5C.]+)/,'')){for(c=location.pathname;c;c=c.replace(/.$/,'')){document.cookie=(a[e]+'; domain='+b+'; path='+c+'; expires='+new Date((new Date()).getTime()-1e11).toGMTString());}}}})())");
                    Close();
                    GlobalLogger.Instance.Info("Получен токен пользователя");
                }
                if (Array.IndexOf(dict.AllKeys, "error") != -1)
                {
                    DialogResult = false;
                    WebView.Navigate("javascript:void((function(){var a,b,c,e,f;f=0;a=document.cookie.split('; ');for(e=0;e<a.length&&a[e];e++){f++;for(b='.'+location.host;b;b=b.replace(/^(?:%5C.|[^%5C.]+)/,'')){for(c=location.pathname;c;c=c.replace(/.$/,'')){document.cookie=(a[e]+'; domain='+b+'; path='+c+'; expires='+new Date((new Date()).getTime()-1e11).toGMTString());}}}})())");
                    Close();
                    GlobalLogger.Instance.Info("Не удалось получить токен пользователя");
                }
            }
            catch
            {
                DialogResult = false;
                GlobalLogger.Instance.Error("Произошла ошибка при авторизации пользователя");
            }
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            WebView.Navigate(_vm.GetAuthUrl());
        }
    }
}
