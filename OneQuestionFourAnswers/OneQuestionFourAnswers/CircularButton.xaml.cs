using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Input;
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

        public ICommand ControlCommand
        {
            get { return (ICommand) GetValue(ControlCommandProperty); }
            set
            {
                SetValue(ControlCommandProperty, value);
                DoPropertyChanged("ControlCommand");
            }
        }

        public static DependencyProperty ControlCommandProperty =
            DependencyProperty.Register("ControlCommand", typeof(ICommand), typeof(CircularButton));

        public static DependencyProperty DisableButtonProperty =
            DependencyProperty.Register("DisableButton", typeof(bool), typeof(CircularButton), new UIPropertyMetadata(false, Refresh));

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

        public static void Refresh(DependencyObject property, DependencyPropertyChangedEventArgs args)
        {
            CircularButton circularButton = (CircularButton)property;
            circularButton.DisableButton = (bool)args.NewValue;
            circularButton.DoPropertyChanged("DisableButton");
            circularButton.DoPropertyChanged("BackgroundImageActive");
            circularButton.DoPropertyChanged("BackgroundImage");
            circularButton.DoPropertyChanged("IsEnabled");
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