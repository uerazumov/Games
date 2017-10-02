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
    /// Логика взаимодействия для StrechTextBlock.xaml
    /// </summary>
    public partial class StrechTextBlock : UserControl, INotifyPropertyChanged
    {
        public StrechTextBlock()
        {
            InitializeComponent();
            DataContext = this;
        }
        public static DependencyProperty TextBlockTextProperty =
            DependencyProperty.Register("TextBlockText", typeof(string), typeof(StrechTextBlock));

        public string TextBlockText
        {
            get
            {
                return (string)GetValue(TextBlockTextProperty);
            }

            set
            {
                SetValue(TextBlockTextProperty, value);
                DoPropertyChanged("TextBlockText");
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
