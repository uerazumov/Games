using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class CircularButton : UserControl, INotifyPropertyChanged
    {
        public CircularButton()
        {
            InitializeComponent();
            DataContext = this;
            DisableButton = false;
        }

        public ImageBrush DisableBackgroundImage { get; set; }
        public ImageBrush DisableBackgroundImageActive { get; set; }
        public ImageBrush EnableBackgroundImage { get; set; }
        public ImageBrush EnableBackgroundImageActive { get; set; }



        public bool _disablebutton { get; set; }
        public bool DisableButton
        {
            get
            {
                return _disablebutton;
            }
            set
            {
                _disablebutton = value;
                DoPropertyChanged("DisableButton");
                DoPropertyChanged("BackgroundImageActive");
                DoPropertyChanged("BackgroundImage");
            }
        }

        public ImageBrush BackgroundImageActive
        {
            get
            {
                return DisableButton ? DisableBackgroundImageActive : EnableBackgroundImageActive;
            }
        }

        public ImageBrush BackgroundImage
        {
            get
            {
                return DisableButton ? DisableBackgroundImage : EnableBackgroundImage;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void DoPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
