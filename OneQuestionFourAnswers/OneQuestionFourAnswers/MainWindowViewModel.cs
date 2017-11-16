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

        public enum ResultType
        {
            Correct,
            Incorrect,
            IncorrectNewRecord
        }

        private int _width;
        private int _heigth;

        private int _questionFontSize;

        public int QuestionFontSize
        {
            get { return _questionFontSize; }
            set
            {
                _questionFontSize = value;
                DoPropertyChanged("QuestionFontSize");
            }
        }

        private int _infoFontSize;

        public int InfoFontSize
        {
            get { return _infoFontSize; }
            set
            {
                _infoFontSize = value;
                DoPropertyChanged("InfoFontSize");
            }
        }

        private int _mainMenuFontSize;

        public int MainMenuFontSize
        {
            get { return _mainMenuFontSize; }
            set
            {
                _mainMenuFontSize = value;
                DoPropertyChanged("MainMenuFontSize");
            }
        }

        private int _answerFontSize;

        public int AnswerFontSize
        {
            get { return _answerFontSize; }
            set
            {
                _answerFontSize = value;
                DoPropertyChanged("AnswerFontSize");
            }
        }

        public delegate void TimeoutDelegate();

        public event TimeoutDelegate Timeout;

        public event TimeoutDelegate Time10Sec;

        public MainWindowViewModel()
        {
        }

        private readonly BussinesLogic.FileProcessing _fp = new BussinesLogic.FileProcessing();
        private TimeSpan _time;

        public string Time => _time.ToString(@"mm\:ss");

        private LibraryClass.RecordsTable _tableOfRecords;

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

        public LibraryClass.QuestionAnswers QuestionAnswers => _questionAnswers;

        private StrechableButton.StateType[] _answersState;

        public StrechableButton.StateType[] AnswersState
        {
            get { return _answersState; }
            set
            {
                _answersState = value;
                DoPropertyChanged("AnswersState");
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

        private bool _answerIsSelect;

        public bool QuestionIsSelect
        {
            get { return _answerIsSelect; }
            set
            {
                _answerIsSelect = value;
                DoPropertyChanged("QuestionIsSelect");
                if (_answerIsSelect)
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
            _timer?.Stop();
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
                {
                    if (_time == TimeSpan.Zero)
                    {
                        StopTimer();
                        Timeout?.Invoke();
                        return;
                    }
                    if (_time == new TimeSpan(0,0,13)) Time10Sec?.Invoke();
                    _time = _time.Add(TimeSpan.FromSeconds(-1));
                    DoPropertyChanged("Time");
                },
                Application.Current.Dispatcher);
            _timer.Start();
        }

        private void UseHintTwoAnswers()
        {
            var answersMask = _fp.HintTwoAnswers(_questionAnswers);
            for (var i = 0; i != 4; i++)
            {
                if (!answersMask[i])
                {
                    AnswersState[i] = StrechableButton.StateType.Inactive;
                }
            }
            DoPropertyChanged("AnswersState");
        }

        public ResultType IsCorrectAnswer(int? index)
        {
            _timer.Stop();
            _answerIsSelect = true;
            if ((index == null) || !_fp.CheckAnswer(QuestionAnswers.Answers[(int) index], ref _gameScore))
            {
                return _fp.CheckRecordIsBrocken(_gameScore) ? ResultType.IncorrectNewRecord : ResultType.Incorrect;
            }
            StartNewRound();
            return ResultType.Correct;
        }



        public void CreateNewRecord()
        {
            _newRecord = new LibraryClass.Record(_name, _gameScore);
            _timer.Stop();
            _fp.CreateNewRecord(_newRecord);
        }

        public void UseHintStatistics()
        {
            StatisticsHeight = _fp.HintStatistics(QuestionAnswers);
        }

        private void GetRecordsTable()
        {
            TableOfRecords = _fp.GetRecordsTable();
        }

        private void Update()
        {
            _fp.UpdateBaseOfQuestion();
        }

        private void OpenNewGame()
        {
            QuestionIsSelect = false;
            GameScore = 0;
            Name = "";
            _fp.RefreshQuestions();
            StartNewRound();
        }

        private void StopTimer()
        {
            _timer.Stop();
        }

        public void StartNewRound()
        {
            _time = new TimeSpan(0, 0, 30);
            _questionAnswers = _fp.NewQuestion();
            GetFontSize();
            AnswersState = new[] {StrechableButton.StateType.Active, StrechableButton.StateType.Active, StrechableButton.StateType.Active, StrechableButton.StateType.Active };
            _questionAnswers.Answers[0].Text = "а. " + _questionAnswers.Answers[0].Text;
            _questionAnswers.Answers[1].Text = "б. " + _questionAnswers.Answers[1].Text;
            _questionAnswers.Answers[2].Text = "в. " + _questionAnswers.Answers[2].Text;
            _questionAnswers.Answers[3].Text = "г. " + _questionAnswers.Answers[3].Text;
            DoPropertyChanged("Time");
            DoPropertyChanged("QuestionAnswers");
            DoPropertyChanged("GameScore");
            DoPropertyChanged("AnswersState");
            CountdownTimer();
        }

        public void AnswerIsSelect(int index)
        {
            for (var i = 0; i != 4; i++)
            {
                if (i == index)
                {
                    AnswersState[i] = StrechableButton.StateType.Chosen;
                }
                else
                {
                    AnswersState[i] = StrechableButton.StateType.Inactive;
                }
            }
            DoPropertyChanged("AnswersState");
        }
        public void PaintTrueAnswer()
        {
            for (var i = 0; i != 4; i++)
            {
                if (_questionAnswers.Answers[i].IsCorrect)
                {
                    AnswersState[i] = StrechableButton.StateType.True;
                }
                else
                {
                    AnswersState[i] = StrechableButton.StateType.Wrong;
                }
            }
            DoPropertyChanged("AnswersState");
        }

        public void ChangeWidthAndHeight(int width, int height)
        {
            _width = width;
            _heigth = height;
            _infoFontSize = (_width * 6000 / (_heigth * 319));
            _mainMenuFontSize = (_width * 700 / (_heigth * 16));
            DoPropertyChanged("MainMenuFontSize");
            int i = 0;
        }

        public void GetFontSize()
        {
            _questionFontSize = (int)(_width * 2600 / (_heigth * _questionAnswers.QuestionText.Length));
            if (_questionFontSize > 90)
            {
                _questionFontSize = 90;
            }

            var maxAnswerLength = _questionAnswers.Answers[0].Text.Length;

            for(var i = 1; i < 4; i++)
            {
                if(_questionAnswers.Answers[i].Text.Length > maxAnswerLength)
                {
                    maxAnswerLength = _questionAnswers.Answers[i].Text.Length;
                }
            }

            _answerFontSize = (_width * 2500 / (_heigth * maxAnswerLength));
            if (_answerFontSize > 80)
            {
                _answerFontSize = 80;
            }
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

        private ICommand _doStopTimerCommand;

        public ICommand DoStopTimerCommand
        {
            get
            {
                if (_doStopTimerCommand == null)
                {
                    _doStopTimerCommand = new Command(
                        p => true,
                        p => StopTimer());
                }
                return _doStopTimerCommand;
            }
        }

        private ICommand _doUpdate;

        public ICommand DoUpdate
        {
            get
            {
                if (_doUpdate == null)
                {
                    _doUpdate = new Command(
                        p => true,
                        p => Update());
                }
                return _doUpdate;
            }
        }
    }
}