using System;
using System.ComponentModel;
using System.Windows.Input;
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
            CountdownTimer();
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

        private bool[] _answersMask;

        public bool[] AnswersMask => _answersMask;

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

        private void UseHintTime()
        {
            _time += new TimeSpan(0, 0, 30);
            DoPropertyChanged("Time");
        }

        private void CountdownTimer()
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

        private void UseHintTwoAnswers()
        {
            _answersMask = _fp.HintTwoAnswers(_questionAnswers);
            DoPropertyChanged("AnswersMask");
        }

        private void CheckTheAnswer()
        {
            if (_fp.CheckAnswer(QuestionAnswers.Answers[2], ref _gameScore)) 
                //нужно реализовать определение вопроса по нажатой кнопке
            {
                StartNewRound();
            }
            else
            {
                if(_fp.CheckRecordIsBrocken(_gameScore))
                {
                    //Здесь должно открыться окно ввода имени
                    _newRecord = new LibraryClass.Record(_name, _gameScore);
                    _fp.CreateNewRecord(_newRecord);
                }
                else
                {
                    //Здесь должно открыться окно поражения
                }
            }
        }

        private void UseHintStatistics()
        {
            _statisticsHeight = _fp.HintStatistics(QuestionAnswers);
            DoPropertyChanged("StatisticsHeight");
        }

        private void GetRecordsTable()
        {
            _tableOfRecords = _fp.GetRecordsTable();
            DoPropertyChanged("RecordsTable");
        }

        private void Update()
        {
            _fp.UpdateBaseOfQuestion();
        }

        private void OpenNewGame()
        {
            _questionIsSelect = false;
            _gameScore = 0;
            _name = "";
            StartNewRound();
            DoPropertyChanged("QuestionIsSelect");
            DoPropertyChanged("GameScore");
            DoPropertyChanged("Name");
            DoPropertyChanged("Time");
            DoPropertyChanged("QuestionAnswers");
            DoPropertyChanged("TwoWrongAnswers");
        }

        private void StartNewRound()
        {
            _time = new TimeSpan(0, 0, 30);
            _fp.NewQuestion(out _questionAnswers);
            _answersMask = new[] {true, true, true, true};
            _timer.Start();
        }

        private ICommand _doUseHintTimeCommand;
        public ICommand DoUseHintTimeCommand
        {
            get
            {
                if (_doUseHintTimeCommand == null)
                {
                    _doUseHintTimeCommand = new Command(
                        p => true,
                        p => UseHintTime());
                }
                return _doUseHintTimeCommand;
            }
        }


        //Решить что используем, метод или команду в GamePage.xaml.cs
        private ICommand _doUseHintStatisticsCommand;
        public ICommand DoUseHintStatisticsCommand
        {
            get
            {
                if (_doUseHintStatisticsCommand == null)
                {
                    _doUseHintStatisticsCommand = new Command(
                        p => true,
                        p => UseHintStatistics());
                }
                return _doUseHintStatisticsCommand;
            }
        }

        private ICommand _doUseHintTwoAnswersCommand;
        public ICommand DoUseHintTwoAnswersCommand
        {
            get
            {
                if (_doUseHintTwoAnswersCommand == null)
                {
                    _doUseHintTwoAnswersCommand = new Command(
                        p => true,
                        p => UseHintTwoAnswers());
                }
                return _doUseHintTwoAnswersCommand;
            }
        }

        private ICommand _doOpenNewGameCommand;
        public ICommand DoOpenNewGameCommand
        {
            get
            {
                if (_doOpenNewGameCommand == null)
                {
                    _doOpenNewGameCommand = new Command(
                        p => true,
                        p => OpenNewGame());
                }
                return _doOpenNewGameCommand;
            }
        }

        private ICommand _doGetRecordsTableCommand;
        public ICommand DoGetRecordsTableCommand
        {
            get
            {
                if (_doGetRecordsTableCommand == null)
                {
                    _doGetRecordsTableCommand = new Command(
                        p => true,
                        p => GetRecordsTable());
                }
                return _doGetRecordsTableCommand;
            }
        }
    }
}