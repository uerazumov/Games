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
        private LibraryClass.RecordsTable _tableOfRecords { get; set; }
        public LibraryClass.RecordsTable TableOfRecords
        {
            get
            {
                return _tableOfRecords;
            }
            set
            {
                _tableOfRecords = value;
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
        private LibraryClass.QuestionAnswers _questionAnswers { get; set; }
        public LibraryClass.QuestionAnswers QuestionAnswers
        {
            get
            {
                return _questionAnswers;
            }
            set
            {
                _questionAnswers = value;
                DoPropertyChanged("QuestionAnswers");
            }
        }
        private bool[] _twoWrongAnswers { get; set; }
        public bool[] TwoWrongAnswers
        {
            get
            {
                return _twoWrongAnswers;
            }
            set
            {
                _twoWrongAnswers = value;

                DoPropertyChanged("TwoWrongAnswers");
            }
        }
        private byte[] _statisticsHeight { get; set; }
        public byte[] StatisticsHeight
        {
            get
            {
                return _statisticsHeight;
            }
            set
            {
                _statisticsHeight = value;
                DoPropertyChanged("StatisticsHeight");
            }
        }
        private LibraryClass.Record _newRecord { get; set; }

        private int _gameScore { get; set; }
        public int GameScore
        {
            get
            {
                return _gameScore;
            }
            set
            {
                _gameScore = value;
                DoPropertyChanged("GameScore");
            }
        }
        private bool _questionIsSelect;
        public bool QuestionIsSelect
        {
            get
            {
                return _questionIsSelect;
            }
            set
            {
                _questionIsSelect = value;
                DoPropertyChanged("QuestionIsSelect");
                if (_questionIsSelect)
                {
                    _timer.Stop();
                }
            }
        }
        private DispatcherTimer _timer;

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
            _newRecord = new LibraryClass.Record(_name, _gameScore);
            FP.CreateNewRecord(_newRecord);
        }
        private void DoUseHintTime()
        {
            _time += new TimeSpan(0, 0, 30);
        }
        private void DoCountdownTimer()
        {
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                Time = _time.ToString(@"mm\:ss");
                if (_time == TimeSpan.Zero)
                {
                    _timer.Stop();
                }
                _time = _time.Add(TimeSpan.FromSeconds(-1));
            },
            App.Current.Dispatcher);
            _timer.Start();
        }

        private void DoUseHintTwoAnswers()
        {
            _twoWrongAnswers = FP.HintTwoAnswers(_questionAnswers);
        }
        private void DoChechTheAnswer()
        {
            var score = _gameScore;
            var defeatRecord = false;
            if (FP.CheckAnswer(QuestionAnswers.Answers[2], ref score, ref defeatRecord)) //нужно реализовать определение вопроса по нажатой кнопке
            {
                _gameScore = score;
            }
            else
            {
                //здесь будет обработка того, что нажал пользователь в окне победы или поражения
            }
        }
        private void DoUseHintStatistics()
        {
            _statisticsHeight = FP.HintStatistics(QuestionAnswers);
        }
        private void DoGetRecordsTable()
        {

            _tableOfRecords = FP.GetRecordsTable();
        }
        private void DoUpdate()
        {
            FP.UpdateBaseOfQuestion();
        }
        private void DoOpenNewGame()
        {
            _questionIsSelect = false;
            var newScore = _gameScore;
            var newName = _name;
            FP.StartNewGame(ref newScore, ref newName);
            var newQuestion = _questionAnswers;
            var newTwoWrongAnswers = _twoWrongAnswers;
            var newTime = _time;
            FP.NewQuestion(ref newTime, ref newQuestion, ref newTwoWrongAnswers);
            _time = newTime;
            _gameScore = newScore;
            _name = newName;
            _questionAnswers = newQuestion;
            _twoWrongAnswers = newTwoWrongAnswers;
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
