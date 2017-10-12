using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace OneQuestionFourAnswers
{
    public partial class StrechableButton : INotifyPropertyChanging
    {
        public StrechableButton()
        {
            InitializeComponent();
            DataContext = this;
            State = null;
        }

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

        //public static DependencyProperty ControlClickProperty =
        //    DependencyProperty.Register("ControlClick", typeof(EventHandler), typeof(StrechableButton));

        //public EventHandler ControlClick
        //{
        //    get { return (EventHandler)GetValue(ControlClickProperty); }
        //    set
        //    {
        //        SetValue(ControlClickProperty, value);
        //        DoPropertyChanged("ControlClick");
        //    }
        //}

        public static DependencyProperty ButtonTextProperty =
            DependencyProperty.Register("ButtonText", typeof(string), typeof(StrechableButton));

        public static DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(bool?), typeof(StrechableButton));

        public static DependencyProperty StateBrushProperty =
            DependencyProperty.Register("StateBrush", typeof(Brush), typeof(StrechableButton));

        public bool? State
        {
            get
            {
                return (bool?)GetValue(StateProperty);
            }

            set
            {
                SetValue(StateProperty, value);
                DoPropertyChanged("State");
                DoPropertyChanged("Background");
                DoPropertyChanged("Foregraund");
            }
        }

        public string BackgroundBrush
        {
            get
            {
                if (State == null)
                {
                    return @"VisualResources\Images\ButtonsDisable.png";
                }
                if ((bool)State)
                {
                    return @"VisualResources\Images\ButtonsTrue.png";
                }
                return @"VisualResources\Images\ButtonsChosen.png";
            }
        }

        public Brush Foregraund
        {
            get
            {
                if (State == null)
                {
                    return Brushes.Gray;
                }
                if ((bool)State)
                {
                    return Brushes.ForestGreen;
                }
                return Brushes.DarkOrange;
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
