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
    public partial class SpinButton : UserControl
    {
        private Int32 _value;

        public int Value
        {
            get
            {
                return (Int32)GetValue(ValueProperty);
            }

            set
            {
                SetValue(ValueProperty, value);
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public static DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(Int32), typeof(SpinButton));

        public SpinButton()
        {
            InitializeComponent();
            Value = 5;
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void PlusClick(object sender, RoutedEventArgs e)
        {
            if (Value > 5)
            Value--;
        }

        private void MinusClick(object sender, RoutedEventArgs e)
        {
            Value++;
        }
    }
}
