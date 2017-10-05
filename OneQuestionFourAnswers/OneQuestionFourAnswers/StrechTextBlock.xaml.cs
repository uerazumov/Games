using System.ComponentModel;
using System.Windows;

namespace OneQuestionFourAnswers
{
    public partial class StrecthTextBlock : INotifyPropertyChanged
    {
        public StrecthTextBlock()
        {
            InitializeComponent();
            DataContext = this;
        }
        public static DependencyProperty TextBlockTextProperty =
            DependencyProperty.Register("TextBlockText", typeof(string), typeof(StrecthTextBlock));

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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
