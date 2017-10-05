using System;
using System.ComponentModel;
using System.Windows.Threading;
using Application = System.Windows.Application;

#region ...
// ReSharper disable UnusedMember.Local
#endregion

namespace OneQuestionFourAnswers
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            //CreateRecord = new Command(DoCreateRecord);
            DoOpenNewGame();
            DoUseHintStatistics();
            DoUseHintTwoAnswers();
            DoGetRecordsTable();
            DoCountdownTimer();
        }

        private readonly BussinesLogic.FileProcessing _fp = new BussinesLogic.FileProcessing();
        private TimeSpan _time;

        public string Time => _time.ToString(@"mm\:ss");

        private LibraryClass.RecordsTable _tableOfRecords;

        public LibraryClass.RecordsTable TableOfRecords
        {
            get { return _tableOfRecords; }
            set
            {
                _tableOfRecords = value;
                DoPropertyChanged("RecordsTable");
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                DoPropertyChanged("Name");
            }
        }

        private LibraryClass.QuestionAnswers _questionAnswers;

        public LibraryClass.QuestionAnswers QuestionAnswers
        {
            get { return _questionAnswers; }
            set
            {
                _questionAnswers = value;
                DoPropertyChanged("QuestionAnswers");
            }
        }

        private bool[] _twoWrongAnswers;

        public bool[] TwoWrongAnswers
        {
            get { return _twoWrongAnswers; }
            set
            {
                _twoWrongAnswers = value;

                DoPropertyChanged("TwoWrongAnswers");
            }
        }

        private byte[] _statisticsHeight;

        public byte[] StatisticsHeight
        {
            get { return _statisticsHeight; }
            set
            {
                _statisticsHeight = value;
                DoPropertyChanged("StatisticsHeight");
            }
        }

        private LibraryClass.Record _newRecord;

        private int _gameScore;

        public int GameScore
        {
            get { return _gameScore; }
            set
            {
                _gameScore = value;
                DoPropertyChanged("GameScore");
            }
        }

        private bool _questionIsSelect;

        public bool QuestionIsSelect
        {
            get { return _questionIsSelect; }
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void DoCreateRecord()
        {
            _newRecord = new LibraryClass.Record(_name, _gameScore);
            _fp.CreateNewRecord(_newRecord);
        }

        private void DoUseHintTime()
        {
            _time += new TimeSpan(0, 0, 30);
        }

        private void DoCountdownTimer()
        {
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
                {
                    if (_time == TimeSpan.Zero)
                    {
                        _timer.Stop();
                    }
                    _time = _time.Add(TimeSpan.FromSeconds(-1));
                    DoPropertyChanged("Time");
                },
                Application.Current.Dispatcher);
            _timer.Start();
        }

        private void DoUseHintTwoAnswers()
        {
            _twoWrongAnswers = _fp.HintTwoAnswers(_questionAnswers);
        }

        private void DoChechTheAnswer()
        {
            var score = _gameScore;
            var defeatRecord = false;
            if (_fp.CheckAnswer(QuestionAnswers.Answers[2], ref score, ref defeatRecord)
            ) //нужно реализовать определение вопроса по нажатой кнопке
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
            _statisticsHeight = _fp.HintStatistics(QuestionAnswers);
        }

        private void DoGetRecordsTable()
        {
            _tableOfRecords = _fp.GetRecordsTable();
        }

        private void DoUpdate()
        {
            _fp.UpdateBaseOfQuestion();
        }

        private void DoOpenNewGame()
        {
            _questionIsSelect = false;
            _gameScore = 0;
            _name = "";
            _time = new TimeSpan(0, 0, 30);
            _fp.NewQuestion(out _questionAnswers);
            _twoWrongAnswers = new[] {true, true, true, true};
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