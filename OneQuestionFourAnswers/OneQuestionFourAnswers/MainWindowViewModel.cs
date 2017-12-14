using LibraryClass;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Application = System.Windows.Application;

namespace OneQuestionFourAnswers
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {

        public enum ResultType
        {
            Correct,
            Incorrect,
            Defeat,
            IncorrectNewRecord
        }

        private Visibility _statusbarState;

        public Visibility StatusbarState
        {
            get { return _statusbarState; }
            set
            {
                _statusbarState = value;
                DoPropertyChanged("StatusbarState");
            }
        }

        private StrechableButton.StateType _buttonsState;

        public StrechableButton.StateType ButtonsState
        {
            get { return _buttonsState; }
            set
            {
                _buttonsState = value;
                DoPropertyChanged("ButtonsState");
            }
        }

        private int _width;
        private int _heigth;

        private bool[] _lives;

        public bool[] Lives
        {
            get { return _lives; }
            set
            {
                _lives = value;
                DoPropertyChanged("Lives");
            }
        }

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

        private int _mainMenuFontSize = 1;

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

        private RecordsTable _tableOfRecords;

        public RecordsTable TableOfRecords
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

        private QuestionAnswers _questionAnswers;

        public QuestionAnswers QuestionAnswers => _questionAnswers;

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

        private Record _newRecord;

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

        private bool _answerIsSelected;

        public bool AnswerIsSelected
        {
            get { return _answerIsSelected; }
            set
            {
                _answerIsSelected = value;
                DoPropertyChanged("AnswerIsSelected");
                if (_answerIsSelected)
                {
                    StopTimer();
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
            StopTimer();
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
            AnswerIsSelected = true;
            if (index == null)
            {
                var answer = new Answer("Ответ не был выбран", false);
                _fp.AddChosenAnswer(answer);
                return IsLivesStayed();
            }
            if (!_fp.CheckAnswer(QuestionAnswers.Answers[(int)index]))
            {
                _fp.AddChosenAnswer(QuestionAnswers.Answers[(int)index]);
                return IsLivesStayed();
            }
            GameScore += 10;
            StartNewRound();
            return ResultType.Correct;
        }

        private ResultType IsLivesStayed()
        {
            var lifeIsStayed = LifeIsStayed();
            if (lifeIsStayed)
            {
                UseOneLife();
                StartNewRound();
                return ResultType.Incorrect;
            }
            UseOneLife();
            return _fp.CheckRecordIsBrocken(_gameScore) ? ResultType.IncorrectNewRecord : ResultType.Defeat;
        }

        private void UseOneLife()
        {
            for(var i = 0; i < 3; i++)
            {
                if(_lives[i])
                {
                    Lives[i] = false;
                    DoPropertyChanged("Lives");
                    break;
                }
            }
        }

        public bool LifeIsStayed()
        {
            return _lives[0] | _lives[1] | !_lives[2];
        }

        public void CreateNewRecord()
        {
            _newRecord = new Record(_name, _gameScore);
            StopTimer();
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

        public void CollapsStatusBar()
        {
            StatusbarState = Visibility.Collapsed;
        }

        private void Update()
        {
            new Thread(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ButtonsState = StrechableButton.StateType.Inactive;
                    StatusbarState = Visibility.Visible;
                });
                try
                {
                    _fp.UpdateBaseOfQuestion();
                }
                catch
                {
                    //ошибка обновления БД
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ButtonsState = StrechableButton.StateType.Active;
                    StatusbarState = Visibility.Collapsed;
                });
            }).Start();
        }

        private void OpenNewGame()
        {
            _fp.ClearReport();
            AnswerIsSelected = false;
            Lives = new[] { true, true, true };
            GameScore = 0;
            Name = "Введите Имя";
            _fp.RefreshQuestions();
            CountdownTimer();
            StartNewRound();
        }

        private void StopTimer()
        {
            _timer?.Stop();
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
            _fp.AddUsedQuestion(_questionAnswers);
            DoPropertyChanged("Time");
            DoPropertyChanged("QuestionAnswers");
            DoPropertyChanged("GameScore");
            DoPropertyChanged("AnswersState");
            _timer.Start();
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
        }

        public void GetFontSize()
        {
            if (_heigth < 24)
            {
                _questionFontSize = (int)(_width * 3000 / (_heigth * _questionAnswers.QuestionText.Length));
            }
            else
            {
                _questionFontSize = (int)(_width * 4000 / (_heigth * _questionAnswers.QuestionText.Length));
            }
            if (_questionFontSize > 75)
            {
                _questionFontSize = 75;
            }

            var maxAnswerLength = _questionAnswers.Answers[0].Text.Length;

            for(var i = 1; i < 4; i++)
            {
                if(_questionAnswers.Answers[i].Text.Length > maxAnswerLength)
                {
                    maxAnswerLength = _questionAnswers.Answers[i].Text.Length;
                }
            }

            if (_heigth < 24)
            {
                _answerFontSize = (_width * 2000 / (_heigth * maxAnswerLength));
            }
            else
            {
                _answerFontSize = (_width * 2500 / (_heigth * maxAnswerLength));
            }
            if (_answerFontSize > 60)
            {
                _answerFontSize = 60;
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