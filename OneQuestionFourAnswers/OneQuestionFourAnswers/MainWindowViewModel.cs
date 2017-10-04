using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace OneQuestionFourAnswers
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            // Пробные очки
            GameScore = 30;
            _time = new TimeSpan(0, 1, 0);
            _name = "иван";
            //CreateRecord = new Command(DoCreateRecord);
            DoOpenNewGame();
            DoUseHintStatistics();
            DoUseHintTwoAnswers();
            DoGetRecordsTable();
            DoCountdownTimer();
        }
        BussinesLogic.FileProcessing FP = new BussinesLogic.FileProcessing();
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
            FP.CreateNewRecord(NewRecord);
        }
        private void DoUseHintTime()
        {
            //Здесь будет метод прибавляющий минуту к игровому времени
        }

        private void DoCountdownTimer()
        {
            DispatcherTimer _timer = new DispatcherTimer();
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                Time = _time.ToString(@"mm\:ss");
                if (_time == TimeSpan.Zero) _timer.Stop();
                _time = _time.Add(TimeSpan.FromSeconds(-1));
            }, App.Current.Dispatcher);

            _timer.Start();
        }

        private void DoUseHintTwoAnswers()
        {
            _twowronganswers = FP.HintTwoAnswers(QuestionPlusAnswers);
        }
        private void DoChechTheAnswer()
        {
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
            _statisticsheight = FP.HintStatistics(QuestionPlusAnswers);
        }
        private void DoGetRecordsTable()
        {
            
            _tableofrecords = FP.GetRecordsTable();
        }
        private void DoUpdate()
        {
            //Здесь будет метод запускающий обновление базы вопросов
        }
        private void DoOpenNewGame()
        {
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
