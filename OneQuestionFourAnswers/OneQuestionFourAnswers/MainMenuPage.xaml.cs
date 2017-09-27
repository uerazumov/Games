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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OneQuestionFourAnswers
{
    /// <summary>
    /// Логика взаимодействия для MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : Page
    {
        public MainMenuPage()
        {
            InitializeComponent();
        }

        private void ButtonClickStartGame(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new Uri("GamePage.xaml", UriKind.Relative));
        }
        private void ButtonClickInformation(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new Uri("InformationPage.xaml", UriKind.Relative));
        }

        private void ButtonClickRefresh(object sender, RoutedEventArgs e)
        {
            RefreshPopup.IsOpen = true;
            IsEnabled = false;
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).LocationChanged += (obj, arg) =>
            {
                if (!RefreshPopup.IsOpen)
                {
                    return;
                }
                var offset = RefreshPopup.HorizontalOffset;
                RefreshPopup.HorizontalOffset = offset + 1;
                RefreshPopup.HorizontalOffset = offset;
            };
        }
    }
}
