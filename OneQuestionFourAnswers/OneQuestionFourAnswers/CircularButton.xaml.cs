using System.ComponentModel;
using System.Windows.Media;

namespace OneQuestionFourAnswers
{
    public partial class CircularButton : INotifyPropertyChanged
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


        private bool _disableButton;

        public bool DisableButton
        {
            get { return _disableButton; }
            set
            {
                _disableButton = value;
                DoPropertyChanged("DisableButton");
                DoPropertyChanged("BackgroundImageActive");
                DoPropertyChanged("BackgroundImage");
            }
        }

        public ImageBrush BackgroundImageActive => DisableButton ? DisableBackgroundImageActive : EnableBackgroundImageActive;

        public ImageBrush BackgroundImage => DisableButton ? DisableBackgroundImage : EnableBackgroundImage;

        public event PropertyChangedEventHandler PropertyChanged;

        public void DoPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}