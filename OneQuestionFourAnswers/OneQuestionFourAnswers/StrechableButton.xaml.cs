using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

// ReSharper disable once PossibleNullReferenceException

namespace OneQuestionFourAnswers
{
    public partial class StrechableButton : INotifyPropertyChanging
    {
        public enum StateType
        {
            Normal,
            Chosen,
            True,
            Wrong
        }

        public StateType State
        {
            get
            {
                if (GetValue(StateProperty) == null)
                {
                    return StateType.Normal;
                }
                return (StateType)GetValue(StateProperty);
            }
            set
            {
                SetValue(StateProperty, value);
                DoPropertyChanged("State");
                DoPropertyChanged("Background");
                DoPropertyChanged("Foregraund");
            }
        }

        public event RoutedEventHandler Click;

        public StrechableButton()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += (sender, args) =>
            {
                Button.Click += (obj, e) => Click?.Invoke(obj, e);
            };   
        }

        public static DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(StateType), typeof(StrechableButton));

        public static DependencyProperty ControlCommandProperty =
            DependencyProperty.Register("ControlCommand", typeof(ICommand), typeof(StrechableButton));

        public ICommand ControlCommand
        {
            get { return (ICommand)GetValue(ControlCommandProperty); }
            set
            {
                SetValue(ControlCommandProperty, value);
                DoPropertyChanged("ControlCommand");
            }
        }

        public static DependencyProperty ButtonTextProperty =
            DependencyProperty.Register("ButtonText", typeof(string), typeof(StrechableButton));

        public static DependencyProperty StateBrushProperty =
            DependencyProperty.Register("StateBrush", typeof(Brush), typeof(StrechableButton));

        public string BackgroundBrush
        {
            get
            {
                string path = @"VisualResources\Images\ButtonsDisable.png";
                switch (State)
                {
                    case StateType.Normal:
                        path = @"VisualResources\Images\ButtonsDisable.png";
                        break;
                    case StateType.Chosen:
                        path = @"VisualResources\Images\ButtonsChosen.png";
                        break;
                    case StateType.True:
                        path = @"VisualResources\Images\ButtonsTrue.png";
                        break;
                    case StateType.Wrong:
                        path = @"VisualResources\Images\ButtonsFalse.png";
                        break;
                }
                return path;
            }
        }

        public Brush Foregraund
        {
            get
            {
                Brush colour = Brushes.Gray;
                switch (State)
                {
                    case StateType.Normal:
                        colour = Brushes.Gray;
                        break;
                    case StateType.Chosen:
                        colour = Brushes.OrangeRed;
                        break;
                    case StateType.True:
                        colour = Brushes.ForestGreen;
                        break;
                    case StateType.Wrong:
                        colour = Brushes.Red;
                        break;
                }
                return colour;
            }
        }

        public string ButtonText
        {
            get
            {
                return (string)GetValue(ButtonTextProperty);
            }

            set
            {
                SetValue(ButtonTextProperty, value);
                DoPropertyChanged("ButtonText");
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;
        public void DoPropertyChanged(string name)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(name));
        }
    }
}
