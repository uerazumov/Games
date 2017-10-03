using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Input;

namespace OneQuestionFourAnswers
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            TwoWrongAnswers = new bool[] { true, true, true, true };
            // Пробный вопрос
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
            // Пробный список высот
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
            // Пробные очки
            GameScore = 30;
            // Пробная таблица рекордов
            LibraryClass.Record First = new LibraryClass.Record("Player 1", 500);
            LibraryClass.Record Second = new LibraryClass.Record("Player 2", 400);
            LibraryClass.Record Third = new LibraryClass.Record("Player 3", 300);
            List<LibraryClass.Record> listR = new List<LibraryClass.Record>();
            listR.Add(First);
            listR.Add(Second);
            listR.Add(Third);
            TableOfRecords = new LibraryClass.RecordsTable(listR);
            //CreateRecord = new Command(DoCreateRecord);
        }
        //public DateTime _time { get; set; }
        //public DateTime Time
        //{
        //    get
        //    {
        //        return _time;
        //    }
        //    set
        //    {
        //        _time = value;
        //        DoPropertyChanged("Time");
        //    }
        //}
        private LibraryClass.RecordsTable _tableofrecords { get; set; }
        public LibraryClass.RecordsTable TableOfRecords
        {
            get
            {
                return _tableofrecords;
            }
            set
            {
                _tableofrecords = value;
                DoPropertyChanged("RecordsTable");
            }
        }
        private string _name { get; set; }
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
        private bool[] _twowronganswers { get; set; }
        public bool[] TwoWrongAnswers
        {
            get
            {
                return _twowronganswers;
            }
            set
            {
                _twowronganswers = value;

                DoPropertyChanged("TwoWrongAnswers");
            }
        }
        private List<byte> _statisticsheight { get; set; }
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
        private LibraryClass.Record _newrecord { get; set; }
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
        private int _gamescore { get; set; }
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
        private LibraryClass.QuestionAnswers _questionplusanswers { get; set; }
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

        private void DoCreateRecord()
        {
            NewRecord = new LibraryClass.Record(Name, GameScore);
            //Здесь будет метод передащий данный рекорд в бизнес логику
        }
        private void DoUseHintTime()
        {
            //Здесь будет метод прибавляющий минуту к игровому времени и вращающий часики
        }
        private void DoUseHintTwoAnswers()
        {
            //Здесь будет метод получающий из БизнесЛогики массив логических элементов
        }
        private void DoChechTheAnswer()
        {
            //Здесь будет метод проверяющий является ли выбранный ответ верным
        }
        private void DoUseHintStatistics()
        {
            //Здесь будет метод получающуй рандомную статистику для одной из подсказок
        }
        private void DoOpenRecordsTable()
        {
            //Здесь будет метод открывающий
        }
        private void DoUpdate()
        {
            //Здесь будет метод запускающий обновление базы вопросов
        }
        private void DoDefeat()
        {
            //Здесь будет метод выводящий окно поражения
        }
        private void DoWin()
        {
            //Здесь будет метод выводящий окно победы
        }
        private void DoCloseGame()
        {
            //Здесь будет метод выводящий окно победы
        }
        private void DoOpenNewGame()
        {
            //Здесь будет метод запускающий новую игру
        }
        private void DoOpenInformation()
        {
            //Здесь будет метод запускающий новую игру
        }
        private void DoOpenMainMany()
        {
            //Здесь будет метод открывающий главное меню
        }
        //private ICommand _doSomething;
        //public ICommand DoSomethingCommand
        //{
        //    get
        //    {
        //        if (_doSomething == null)
        //        {
        //            _doSomething = new Command(
        //                p => this.Ca,
        //                p => DoCreateRecord());
        //        }
        //        return _doSomething;
        //    }
        //}
    }
}
