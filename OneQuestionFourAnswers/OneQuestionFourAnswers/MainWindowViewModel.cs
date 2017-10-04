using System;
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
            // Пробный список высот

            byte[] lb = new byte[]{ 10, 20, 30, 40};

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
            _time = new TimeSpan(0, 1, 0);
            _name = "иван";
            DoOpenNewGame();
            DoUseHintStatistics();
        }
        private TimeSpan _time { get; set; }
        public string Time
        {
            get
            {
                return _time.ToString(@"mm\:ss");
            }
            set
            {
                DoPropertyChanged("Time");
            }
        }
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
        private byte[] _statisticsheight { get; set; }
        public byte[] StatisticsHeight
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
            BussinesLogic.FileProcessing FP = new BussinesLogic.FileProcessing();
            int score = _gamescore;
            bool DefeatReacord = false;
            if (FP.CheckAnswer(QuestionPlusAnswers.Answers[2], ref score, ref DefeatReacord)) //нужно реализовать определение вопроса по нажатой кнопке
            {
                _gamescore = score;
            }
            else
            {
                //здесь будет обработка того, что нажал пользователь в окне победы или поражения
            }
        }
        private void DoUseHintStatistics()
        {
            BussinesLogic.FileProcessing FP = new BussinesLogic.FileProcessing();
            _statisticsheight = FP.HintStatistics(QuestionPlusAnswers);
        }
        private void DoGetRecordsTable()
        {
            //Здесь будет метод передачи рекордов
        }
        private void DoUpdate()
        {
            //Здесь будет метод запускающий обновление базы вопросов
        }
        private void DoOpenNewGame()
        {
            BussinesLogic.FileProcessing FP = new BussinesLogic.FileProcessing();
            int NewScore = GameScore;
            string NewName = Name;
            FP.StartNewGame(ref NewScore, ref NewName);
            LibraryClass.QuestionAnswers NewQuestion = QuestionPlusAnswers;
            bool[] NewTwoWrongAnswers = TwoWrongAnswers;
            TimeSpan NewTime = _time;
            FP.NewQuestion(ref NewTime, ref NewQuestion, ref NewTwoWrongAnswers);
            _time = NewTime;
            _gamescore = NewScore;
            _name = NewName;
            QuestionPlusAnswers = NewQuestion;
            _twowronganswers = NewTwoWrongAnswers;
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
