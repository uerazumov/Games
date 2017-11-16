using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OneQuestionFourAnswers
{
    public partial class LifeIcon : UserControl
    {
        public LifeIcon()
        {
            InitializeComponent();
            DataContext = this;
        }

        public ImageBrush DisableImage = new ImageBrush(new BitmapImage(new Uri(@".\VisualResources\Images\LifeUsedIcon.png", UriKind.Relative)));
        public ImageBrush EnableImage = new ImageBrush(new BitmapImage(new Uri(@".\VisualResources\Images\LifeIcon.png", UriKind.Relative)));
    }
}
