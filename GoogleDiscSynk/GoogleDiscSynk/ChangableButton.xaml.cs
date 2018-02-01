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

namespace GoogleDiscSynk
{
    /// <summary>
    /// Логика взаимодействия для ChangableButton.xaml
    /// </summary>
    public partial class ChangableButton : UserControl
    {
        public ChangableButton()
        {
            DataContext = this;
            InitializeComponent();
        }
        public ImageBrush BackgroundImage { get; set; }
        public ImageBrush BackgroundImageActive { get; set; }
    }
}
