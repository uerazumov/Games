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
using System.Windows.Shapes;

namespace DriverPackCD
{
    /// <summary>
    /// Логика взаимодействия для Notify.xaml
    /// </summary>
    public partial class Notify : Window
    {
        public Notify()
        {
            InitializeComponent();
        }
        

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            CloseButton.ControlButton.Click += CloseWindow;
        }
    }
}
