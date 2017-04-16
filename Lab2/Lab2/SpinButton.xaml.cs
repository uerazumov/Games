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

namespace Lab2
{
    public partial class SpinButton : UserControl, INotifyPropertyChanged
    {
        public static DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(byte), typeof(SpinButton));

        public byte Value
        {
            get
            {
                return (byte)GetValue(ValueProperty);
            }

            set
            {
                SetValue(ValueProperty, value);
                DoPropertyChanged("Value");
            }
        }

        public static DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(byte), typeof(SpinButton));

        public byte Points
        {
            get
            {
                return (byte)GetValue(PointsProperty);
            }

            set
            {
                SetValue(PointsProperty, value);
                DoPropertyChanged("Points");
            }
        }

        private void MinusClick(object sender, RoutedEventArgs e)
        {
            if (Value > 5)
            {
                Value--;
                Points++;
            }
        }

        public SpinButton()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void PlusClick(object sender, RoutedEventArgs e)
        {
            if (Points > 0)
            {
                Value++;
                Points--;
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
