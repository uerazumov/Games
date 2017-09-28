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
    /// Логика взаимодействия для CircularButton.xaml
    /// </summary>
    public partial class CircularButton : UserControl
    {
        public CircularButton()
        {
            InitializeComponent();
            DataContext = this;
        }
        public ImageBrush BackgroundImage { get; set; }
        public ImageBrush BackgroundImageActive { get; set; }
    }
}
