using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel;

namespace OneQuestionFourAnswers 
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            //  Пробный вопрос
            LibraryClass.Answer AnswerA = new LibraryClass.Answer("ответ А", false);
            LibraryClass.Answer AnswerB = new LibraryClass.Answer("ответ Б", false);
            LibraryClass.Answer AnswerC = new LibraryClass.Answer("ответ В", true);
            LibraryClass.Answer AnswerD = new LibraryClass.Answer("ответ Г", false);
            List<LibraryClass.Answer> AnswerList = new List<LibraryClass.Answer>();
            AnswerList.Add(AnswerA);
            AnswerList.Add(AnswerB);
            AnswerList.Add(AnswerC);
            AnswerList.Add(AnswerD);
            QuestionPlusAnswers = new LibraryClass.QuestionAnswers("Текст вопроса", AnswerList);
            // /Пробный вопрос
            //  Пробный список высот
            byte a = 10;
            byte b = 20;
            byte c = 30;
            byte d = 40;
            List<byte> lb = new List<byte>();
            lb.Add(a);
            lb.Add(b);
            lb.Add(c);
            lb.Add(d);
            StatisticsHeight = lb;
            //  /Пробный список высот
            GameScore = 30;
        }
        public DateTime _time { get; set; }
        public DateTime Time
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
                DoPropertyChanged("Time");
            }
        }
        public string _name { get; set; }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                DoPropertyChanged("Name");
            }
        }
        public List<byte> _statisticsheight { get; set; }
        public List<byte> StatisticsHeight
        {
            get
            {
                return _statisticsheight;
            }
            set
            {
                _statisticsheight = value;
                DoPropertyChanged("StatisticsHeight");
            }
        }
        public LibraryClass.Record _newrecord { get; set; }
        public LibraryClass.Record NewRecord
        {
            get
            {
                return _newrecord;
            }
            set
            {
                _newrecord = value;
                DoPropertyChanged("NewRecord");
            }
        }
        public int _gamescore { get; set; }
        public int GameScore
        {
            get
            {
                return _gamescore;
            }
            set
            {
                _gamescore = value;
                DoPropertyChanged("GameScore");
            }
        }
        public LibraryClass.QuestionAnswers _questionplusanswers { get; set; }
        public LibraryClass.QuestionAnswers QuestionPlusAnswers
        {
            get
            {
                return _questionplusanswers;
            }
            set
            {
                _questionplusanswers = value;
                DoPropertyChanged("QuestionPlusAnswers");
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
