using LibraryClass;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Application = System.Windows.Application;
using LoggingService;
using System.Windows.Navigation;
using System.Windows.Controls;

namespace OneQuestionFourAnswers
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public enum DialogWindowType
        {
            DefeatWindow,
            ExitTheGame,
            ExitWindow,
            NewRecordWindow,
            StatisticWindow
        }

        private Grid _hints;

        private byte _countOfAwaitings;

        private MainWindow _mainWindow;

        private NavigationService _navigationService;

        private static Window _dialogWindow;

        public enum ResultType
        {
            Correct,
            Incorrect,
            Defeat,
            IncorrectNewRecord
        }

        private Visibility[] _recordMessagesState;
        //0 - Nitice; 1 - Error; 2 - Succesfully
        public Visibility[] RecordMessagesState
        {
            get { return _recordMessagesState; }
            set
            {
                _recordMessagesState = value;
                DoPropertyChanged("RecordMessagesState");
            }
        }

        private Visibility[] _reportMessagesState;
        //0 - Error; 1 - Succesfully
        public Visibility[] ReportMessagesState
        {
            get { return _reportMessagesState; }
            set
            {
                _reportMessagesState = value;
                DoPropertyChanged("ReportMessagesState");
            }
        }

        private Visibility[] _vkMessagesState;
        //0 - Error; 1 - Succesfully
        public Visibility[] VkMessagesState
        {
            get { return _vkMessagesState; }
            set
            {
                _vkMessagesState = value;
                DoPropertyChanged("VkMessagesState");
            }
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

        private Visibility _waitMessageState;

        public Visibility WaitMessageState
        {
            get { return _waitMessageState; }
            set
            {
                _waitMessageState = value;
                DoPropertyChanged("WaitMessageState");
            }
        }

        private Visibility _userNameBox;

        public Visibility UserNameBox
        {
            get { return _userNameBox; }
            set
            {
                _userNameBox = value;
                DoPropertyChanged("UserNameBox");
            }
        }

        private bool[] _twoAnswersHintButtonState;
        // 0 - IsEnabled, 1 - DisabledButton
        public bool[] TwoAnswersHintButtonState
        {
            get { return _twoAnswersHintButtonState; }
            set
            {
                _twoAnswersHintButtonState = value;
                DoPropertyChanged("TwoAnswersHintButtonState");
            }
        }

        private bool[] _timeHintButtonState;
        // 0 - IsEnabled, 1 - DisabledButton
        public bool[] TimeHintButtonState
        {
            get { return _timeHintButtonState; }
            set
            {
                _timeHintButtonState = value;
                DoPropertyChanged("TimeHintButtonState");
            }
        }

        private bool[] _statisticHintButtonState;
        // 0 - IsEnabled, 1 - DisabledButton
        public bool[] StatisticHintButtonState
        {
            get { return _statisticHintButtonState; }
            set
            {
                _statisticHintButtonState = value;
                DoPropertyChanged("StatisticHintButtonState");
            }
        }

        private bool _logOutButtonState;

        public bool LogOutButtonState
        {
            get { return _logOutButtonState; }
            set
            {
                _logOutButtonState = value;
                DoPropertyChanged("LogOutButtonState");
            }
        }

        private bool _saveRecordIntoVKButtonState;

        public bool SaveRecordIntoVKButtonState
        {
            get { return _saveRecordIntoVKButtonState; }
            set
            {
                _saveRecordIntoVKButtonState = value;
                DoPropertyChanged("SaveRecordIntoVKButtonState");
            }
        }

        private bool _saveStatisticButtonState;

        public bool SaveStatisticButtonState
        {
            get { return _saveStatisticButtonState; }
            set
            {
                _saveStatisticButtonState = value;
                DoPropertyChanged("SaveStatisticButtonState");
            }
        }

        private bool _borderUserNameTextBoxState;

        public bool BorderUserNameTextBoxState
        {
            get { return _borderUserNameTextBoxState; }
            set
            {
                _borderUserNameTextBoxState = value;
                DoPropertyChanged("BorderUserNameTextBoxState");
            }
        }

        private bool _closeButtonState;

        public bool CloseButtonState
        {
            get { return _closeButtonState; }
            set
            {
                _closeButtonState = value;
                DoPropertyChanged("CloseButtonState");
            }
        }

        private bool _saveRecordButtonState;

        public bool SaveRecordButtonState
        {
            get { return _saveRecordButtonState; }
            set
            {
                _saveRecordButtonState = value;
                DoPropertyChanged("SaveRecordButtonState");
            }
        }

        private int _progressBarValue;

        public int ProgressBarValue
        {
            get { return _progressBarValue; }
            set
            {
                _progressBarValue = value;
                DoPropertyChanged("ProgressBarValue");
            }
        }


        private bool _logInStatus;

        public bool LogInStatus
        {
            get { return _logInStatus; }
            set
            {
                _logInStatus = value;
                DoPropertyChanged("LogInStatus");
            }
        }

        private StrechableButton.StateType[] _buttonsState = new StrechableButton.StateType[2];

        public StrechableButton.StateType[] ButtonsState
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
            }
        }

        private DispatcherTimer _timer;

        public event PropertyChangedEventHandler PropertyChanged;

        public void DoPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public MainWindowViewModel()
        {
            if (IsBaseEmpty())
            {
                Update();
            }
            ChangeSettings();
            ProgressBarValue = 0;
        }

        public void ChangeSettings()
        {
            if (IsTokenExist())
            {
                LogInStatus = true;
                LogOutButtonState = false;
                var name = GetUserName();
                if (name != null)
                {
                    Name = name;
                    UserNameBox = Visibility.Visible;
                }
                else
                {
                    UserNameBox = Visibility.Collapsed;
                }
                GlobalLogger.Instance.Info("Имя пользователя было установлено");
            }
            else
            {
                UserNameBox = Visibility.Collapsed;
                LogInStatus = false;
                LogOutButtonState = true; ;
                UserNameBox = Visibility.Collapsed;
                Name = "Введите Имя";
            }
        }

        private void UseHintTime()
        {
            GlobalLogger.Instance.Info("Пользователь использовал подсказку Дополнительное Время");
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
                        AnswerIsSelectAnimation(null);
                        GlobalLogger.Instance.Info("Истекло время за которое можно ответить на вопрос");
                        return;
                    }
                    if (_time == new TimeSpan(0, 0, 13))
                    {
                        _mainWindow?.PlaySound(MainWindow.SoundType.Ends10SecSound);
                        GlobalLogger.Instance.Info("Осталось 13 (10) секунд на выбор ответа");
                    }
                    _time = _time.Add(TimeSpan.FromSeconds(-1));
                    DoPropertyChanged("Time");
                },
                Application.Current.Dispatcher);
        }

        private void UseHintTwoAnswers()
        {
            GlobalLogger.Instance.Info("Пользователь использовал подсказку Убрать Два Неверных Ответа");
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
            GlobalLogger.Instance.Info("Запущена проверка ответа");
            AnswerIsSelected = true;
            if (index == null)
            {
                var answer = new Answer("Ответ не был выбран", false);
                GlobalLogger.Instance.Info("Раунд проигран. Пользователь не выбрал ответ");
                _fp.AddChosenAnswer(answer);
                return IsLivesStayed();
            }
            if (!_fp.CheckAnswer(QuestionAnswers.Answers[(int)index]))
            {
                _fp.AddChosenAnswer(QuestionAnswers.Answers[(int)index]);
                GlobalLogger.Instance.Info("Раунд проигран. Пользователь выбрал неверный вопрос");
                return IsLivesStayed();
            }
            GameScore += 10;
            _fp.AddChosenAnswer(QuestionAnswers.Answers[(int)index]);
            GlobalLogger.Instance.Info("Раунд выигран. Пользователь выбрал верный вопрос");
            StartNewRound();
            return ResultType.Correct;
        }

        private ResultType IsLivesStayed()
        {
            GlobalLogger.Instance.Info("Запущена проверка - Остались ли жизни");
            var lifeIsStayed = LifeIsStayed();
            if (lifeIsStayed)
            {
                GlobalLogger.Instance.Info("Жизни остались");
                UseOneLife();
                StartNewRound();
                return ResultType.Incorrect;
            }
            UseOneLife();
            GlobalLogger.Instance.Info("Жизней не осталось");
            return _fp.CheckRecordIsBrocken(_gameScore) ? ResultType.IncorrectNewRecord : ResultType.Defeat;
        }

        private void UseOneLife()
        {
            GlobalLogger.Instance.Info("Была потрачена одна жизнь");
            for (var i = 0; i < 3; i++)
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
            GlobalLogger.Instance.Info("Запущена проверка - Потрачены ли жизни");
            return _lives[0] | _lives[1] | !_lives[2];
        }

        public bool CreateNewRecord()
        {
            GlobalLogger.Instance.Info("Создан новый рекорд");
            _newRecord = new Record(_name, _gameScore);
            StopTimer();
            return _fp.CreateNewRecord(_newRecord);
        }

        public void UseHintStatistics()
        {
            GlobalLogger.Instance.Info("Пользователь использовал подсказку Показать Статистику");
            StatisticsHeight = _fp.HintStatistics(QuestionAnswers);
        }

        private void GetRecordsTable()
        {
            GlobalLogger.Instance.Info("Была получена Таблица Рекордов");
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
                    try
                    {
                        GlobalLogger.Instance.Info("Было запущено обновление");
                        _buttonsState[0] = StrechableButton.StateType.Inactive;
                        _buttonsState[1] = StrechableButton.StateType.Inactive;
                        DoPropertyChanged("ButtonsState");
                        StatusbarState = Visibility.Visible;
                    }
                    catch (Exception e)
                    {
                        GlobalLogger.Instance.Error("Было вызвано исключение" + e.Message + "старте обновления");
                    }
                });
                try
                {
                    _fp.UpdateBaseOfQuestion();
                }
                catch (Exception e)
                {
                    GlobalLogger.Instance.Error("Произошла ошибка " + e.Message + " при обновлении Базы Вопросов");
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        if (!IsBaseEmpty())
                        {
                            _buttonsState[0] = StrechableButton.StateType.Active;
                        }
                        _buttonsState[1] = StrechableButton.StateType.Active;
                        DoPropertyChanged("ButtonsState");
                        CollapsStatusBar();
                        GlobalLogger.Instance.Info("Обновление было завершено");
                    }
                    catch (Exception e)
                    {
                        GlobalLogger.Instance.Error("Было вызвано исключение" + e.Message + "завершении обновления");
                    }
                });
            }).Start();
            new Thread(() =>
            {
                var status = true;
                while (status)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        try
                        {
                            var value = _fp.GetUpdateProcent();
                            if ((value > -1) && (value < 101)) ProgressBarValue = value;
                            if (value == -1) ProgressBarValue = 100;
                        }
                        catch (Exception e)
                        {
                            GlobalLogger.Instance.Error("Было вызвано исключение" + e.Message + "при смене значения progressbar");
                        }
                    });
                    if (ProgressBarValue > 99)
                    {
                        status = false;
                    }
                }
            }).Start();
        }

        private void RefreshBindings()
        {
            TwoAnswersHintButtonState = new bool[2] { true, false };
            StatisticHintButtonState = new bool[2] { true, false };
            TimeHintButtonState = new bool[2] { true, false };
            _countOfAwaitings = 0;
            RecordMessagesState = new Visibility[3] { Visibility.Collapsed, Visibility.Collapsed, Visibility.Collapsed };
            ReportMessagesState = new Visibility[2] { Visibility.Collapsed, Visibility.Collapsed };
            VkMessagesState = new Visibility[2] { Visibility.Collapsed, Visibility.Collapsed };
            WaitMessageState = Visibility.Collapsed;
            TwoAnswersHintButtonState = new bool[2] { true, false };
            SaveRecordIntoVKButtonState = true;
            SaveStatisticButtonState = true;
            BorderUserNameTextBoxState = true;
            CloseButtonState = true;
            SaveRecordButtonState = true;
        }

        private void OpenNewGame()
        {
            RefreshBindings();
            _fp.ClearReport();
            AnswerIsSelected = false;
            Lives = new[] { true, true, true };
            GameScore = 0;
            _fp.RefreshQuestions();
            GlobalLogger.Instance.Info("Была начата новая игра");
            CountdownTimer();
            StartNewRound();
        }

        private void StopTimer()
        {
            _timer?.Stop();
        }

        private void StartNewRound()
        {
            _time = new TimeSpan(0, 0, 30);
            _questionAnswers = _fp.NewQuestion();
            GlobalLogger.Instance.Info("Был начат новый раунд");
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

  
        public void AnswerIsSelect(int? index)
        {
            for (var i = 0; i != 4; i++)
            {
                if (index == null)
                {
                    AnswersState[i] = StrechableButton.StateType.Inactive;
                }
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
            GlobalLogger.Instance.Info("Был выбран ответ");
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
            GlobalLogger.Instance.Info("Был выделен верный ответ");
        }

        public void ChangeWidthAndHeight(int width, int height)
        {
            _width = width;
            _heigth = height;
            if (_heigth < 800)
            {
                _infoFontSize = (_width * 6000 / (_heigth * 319));
            }
            else
            {
                _infoFontSize = (_width * 8000 / (_heigth * 319));
            }
            _mainMenuFontSize = (_width * 700 / (_heigth * 16));
            DoPropertyChanged("MainMenuFontSize");
            GlobalLogger.Instance.Info("Была произведена подгонка размеров шрифта основных меню под размеры экрана");
        }

        public void GetFontSize()
        {
            if (_heigth < 800)
            {
                _questionFontSize = (int)(_width * 3000 / (_heigth * _questionAnswers.QuestionText.Length));
                if (_questionFontSize > 65)
                {
                    _questionFontSize = 65;
                }
            }
            else
            {
                _questionFontSize = (int)(_width * 4000 / (_heigth * _questionAnswers.QuestionText.Length));
                if (_questionFontSize > 75)
                {
                    _questionFontSize = 75;
                }
            }
            if (_questionAnswers.QuestionText.Length > 130)
            {
                _questionFontSize -= 10;
            }

            var maxAnswerLength = _questionAnswers.Answers[0].Text.Length;

            for(var i = 1; i < 4; i++)
            {
                if(_questionAnswers.Answers[i].Text.Length > maxAnswerLength)
                {
                    maxAnswerLength = _questionAnswers.Answers[i].Text.Length;
                }
            }

            if (_heigth < 800)
            {
                _answerFontSize = (_width * 1500 / (_heigth * maxAnswerLength));
            }
            else
            {
                _answerFontSize = (_width * 2500 / (_heigth * maxAnswerLength));
            }
            if (_answerFontSize > 60)
            {
                _answerFontSize = 60;
            }
            GlobalLogger.Instance.Info("Была произведена подгонка размеров шрифтов вопроса и ответов под размеры экрана");
        }

        public bool? CreateReport()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Report";
            dlg.DefaultExt = ".xlsx";
            dlg.Filter = "Таблица Excel (.xlsx)|*.xlsx";
            var result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                return _fp.CreateReport(filename);
            };
            return null;
        }

        private bool IsBaseEmpty()
        {
            return _fp.IsBaseEmpty();
        }

        public bool CreateRec()
        {
            return _fp.CreateRec(_gameScore);
        }

        public string GetAuthUrl()
        {
            return _fp.GetAuthUrl();
        }

        public void SaveToken(string token, string userID)
        {
            _fp.SaveToken(token, userID);
            LogInStatus = true;
        }

        public bool IsTokenExist()
        {
            return _fp.IsTokenExist();
        }

        private string GetUserName()
        {
            return _fp.GetUserName();
        }

        public void LogOut()
        {
            _fp.LogOut();
            LogInStatus = false;
        }

        public static bool? OpenDialogWindow(DialogWindowType type)
        {
            if (_dialogWindow != null)
            {
                _dialogWindow.Close();
                GlobalLogger.Instance.Info("Диалоговое окно было закрыто, всвязи с открытием диалгового окна типа " + type.ToString());
            }
            switch (type)
            {
                case DialogWindowType.DefeatWindow:
                    _dialogWindow = new DefeatWindow { Owner = App.Current.MainWindow };
                    break;
                case DialogWindowType.ExitTheGame:
                    _dialogWindow = new ExitTheGame { Owner = App.Current.MainWindow };
                    break;
                case DialogWindowType.ExitWindow:
                    _dialogWindow = new ExitWindow { Owner = App.Current.MainWindow };
                    break;
                case DialogWindowType.NewRecordWindow:
                    _dialogWindow = new NewRecordWindow { Owner = App.Current.MainWindow };
                    break;
                case DialogWindowType.StatisticWindow:
                    _dialogWindow = new StatisticsWindow { Owner = App.Current.MainWindow };
                    break;
            }
            _dialogWindow.ShowDialog();
            GlobalLogger.Instance.Info("Было открыто диалоговое окно типа " + type.ToString());
            var result = _dialogWindow.DialogResult;
            GlobalLogger.Instance.Info("Диалоговое окно было закрыто пользователем с результатом " + result.ToString());
            return result;
        }

        private void ReturnFalseResultClick()
        {
            _dialogWindow.DialogResult = false;
            _dialogWindow.Close();
        }

        private void ReturnTrueResultClick()
        {
            _dialogWindow.DialogResult = true;
            _dialogWindow.Close();
        }

        private void ExitGamePageClick()
        {
            _dialogWindow.DialogResult = false;
            _dialogWindow.Close();
            StopTimer();
        }

        private void StartNewGame()
        {
            _mainWindow?.PlaySound(MainWindow.SoundType.NewGameSound);
            _dialogWindow.DialogResult = true;
            _dialogWindow.Close();
            OpenNewGame();
        }

        public void AssignMainWindow(MainWindow mw)
        {
            _mainWindow = mw;
        }

        private void StartNewGameClick()
        {
            _navigationService?.Navigate(new Uri("GamePage.xaml", UriKind.Relative));
            GlobalLogger.Instance.Info("Была открыта страница Игра");
            OpenNewGame();
        }

        private void InformationClick()
        {
            _navigationService?.Navigate(new Uri("InformationPage.xaml", UriKind.Relative));
            GlobalLogger.Instance.Info("Была открыта страница Информация");
        }

        private void HighscoreClick()
        {
            GetRecordsTable();
            _navigationService?.Navigate(new Uri("HighscoreTablePage.xaml", UriKind.Relative));
            GlobalLogger.Instance.Info("Была открыта страница Таблица Рекордов");
        }

        private void BackToMenuClick()
        {
            _navigationService?.Navigate(new Uri("MainMenuPage.xaml", UriKind.Relative));
            GlobalLogger.Instance.Info("Была открыта страница Главное Меню");
        }

        private void OnMainMenuLoaded(object page)
        {
            ChangeSettings();
            _navigationService = NavigationService.GetNavigationService(page as MainMenuPage);
        }

        private void CloseDialog()
        {
            _dialogWindow.Close();
        }

        private void AssignHintsGrid(object grid)
        {
            _hints = grid as Grid;
        }

        private void LogOutClick()
        {
            LogOut();
            ChangeSettings();
            ChangeSettings();
        }

        private void SaveStatisticClick()
        {
            new Thread(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    CloseButtonState = false;
                    SaveStatisticButtonState = false;
                    WaitMessageState = Visibility.Visible;
                    _countOfAwaitings++;
                });
                var result = CreateReport();
                if (result == true)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ReportMessagesState[0] = Visibility.Collapsed;
                        ReportMessagesState[1] = Visibility.Visible;
                        DoPropertyChanged("ReportMessagesState");
                    });
                }
                else if (result == false)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        SaveStatisticButtonState = true;
                        ReportMessagesState[0] = Visibility.Visible;
                        DoPropertyChanged("ReportMessagesState");
                    });
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (_countOfAwaitings - 1 == 0)
                    {
                        WaitMessageState = Visibility.Collapsed;
                        CloseButtonState = true;
                    }
                    _countOfAwaitings--;
                });
            }).Start();
        }

        private void PostPhotoClick()
        {
            new Thread(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    CloseButtonState = false;
                    SaveRecordIntoVKButtonState = false;
                    WaitMessageState = Visibility.Visible;
                    _countOfAwaitings++;
                });
                bool? webResult = true;
                if (!IsTokenExist())
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        WebBrowser web = new WebBrowser();
                        web.ShowDialog();
                        web.Owner = _dialogWindow.Owner;
                        webResult = web.DialogResult;
                    });
                }
                if (webResult != null)
                {
                    if ((webResult ?? false) && CreateRec())
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            VkMessagesState[0] = Visibility.Collapsed;
                            VkMessagesState[1] = Visibility.Visible;
                            DoPropertyChanged("VkMessagesState");
                        });
                    }
                    else
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            SaveRecordIntoVKButtonState = true;
                            VkMessagesState[0] = Visibility.Visible;
                            DoPropertyChanged("VkMessagesState");
                        });
                    }
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (_countOfAwaitings - 1 == 0)
                    {
                        WaitMessageState = Visibility.Collapsed;
                        CloseButtonState = true;
                    }
                    _countOfAwaitings--;
                });
            }).Start();
        }

        private void SaveRecordClick(object textBox)
        {
            (textBox as TextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
            if (!Validation.GetHasError(textBox as TextBox))
            {
                new Thread(() =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        CloseButtonState = false;
                        SaveRecordButtonState = false;
                        WaitMessageState = Visibility.Visible;
                        _countOfAwaitings++;
                    });
                    if (CreateNewRecord())
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            BorderUserNameTextBoxState = false;
                            RecordMessagesState[0] = Visibility.Collapsed;
                            RecordMessagesState[1] = Visibility.Collapsed;
                            RecordMessagesState[2] = Visibility.Visible;
                            DoPropertyChanged("RecordMessagesState");
                        });
                    }
                    else
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            SaveRecordButtonState = true;
                            RecordMessagesState[0] = Visibility.Collapsed;
                            RecordMessagesState[1] = Visibility.Visible;
                            DoPropertyChanged("RecordMessagesState");
                        });
                    };
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (_countOfAwaitings - 1 == 0)
                        {
                            WaitMessageState = Visibility.Collapsed;
                            CloseButtonState = true;
                        }
                        _countOfAwaitings--;
                    });
                }).Start();
            }
            RecordMessagesState[0] = Visibility.Visible;
            DoPropertyChanged("RecordMessagesState");
        }

        private void ExitGamePageButtonClick()
        {
            var close = OpenDialogWindow(DialogWindowType.ExitTheGame) ?? true;
            if (!close)
            {
                BackToMenuClick();
            }
        }

        private void ButtonClickTwoAnswers()
        {
            _mainWindow?.PlaySound(MainWindow.SoundType.TwoAnswersSound);
            TwoAnswersHintButtonState[1] = true;
            TwoAnswersHintButtonState[0] = false;
            DoPropertyChanged("TwoAnswersHintButtonState");
            UseHintTwoAnswers();
        }

        private void OnTextBoxFocused(object textBox)
        {
            (textBox as TextBox).Focus();
            (textBox as TextBox).SelectAll();
            if (!Validation.GetHasError(textBox as TextBox))
            {
                return;
            }
            (textBox as TextBox).Text = "";
            var property = TextBox.TextProperty;
            Validation.ClearInvalid((textBox as TextBox).GetBindingExpression(property));
            RecordMessagesState[0] = Visibility.Visible;
        }

        private void RefreshMainMenuPage()
        {
            LogOutButtonState = !_logInStatus;
            if (LogOutButtonState)
            {
                UserNameBox = Visibility.Collapsed;
            }
            else
            {
                if (Name != "Введите Имя") UserNameBox = Visibility.Visible;
                else UserNameBox = Visibility.Collapsed;
            }
        }

        private void ButtonClickStatistics()
        {
            _mainWindow.PlaySound(MainWindow.SoundType.StatisticSound);
            UseHintStatistics();
            StatisticHintButtonState[1] = true;
            StatisticHintButtonState[0] = false;
            DoPropertyChanged("StatisticHintButtonState");
            OpenDialogWindow(DialogWindowType.StatisticWindow);
        }

        private void ButtonClickTime(object clock)
        {
            _mainWindow.PlaySound(MainWindow.SoundType.TimeAddedSound);
            UseHintTime();
            TimeHintButtonState[1] = true;
            TimeHintButtonState[0] = false;
            DoPropertyChanged("TimeHintButtonState");
        }

        private void AnswerButtonClick(object button)
        {
            StopTimer();
            var chosedButton = (StrechableButton)button;
            var index = Convert.ToInt16(chosedButton.Tag);
            AnswerIsSelectAnimation(index);
        }

        private void AnswerIsSelectAnimation( int? index)
        {
            AnswerIsSelect(index);
            _hints.IsEnabled = false;
            var counter = 0;
            var timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                counter++;
                if (counter == 1)
                {
                    PaintTrueAnswer();
                }
                if (counter == 2)
                {
                    CheckAnswer(index);
                    _hints.IsEnabled = true;
                    return;
                }
            },
            Application.Current.Dispatcher);
        }

        private void CheckAnswer(int? index)
        {

            switch (IsCorrectAnswer(index))
            {
                case ResultType.Correct:
                    {
                        _mainWindow?.PlaySound(MainWindow.SoundType.CorrectAnswer);
                    }
                    break;
                case ResultType.Incorrect:
                    {
                        _mainWindow?.PlaySound(MainWindow.SoundType.LifeIsBroken);
                    }
                    break;
                case ResultType.Defeat:
                    {
                        _mainWindow?.PlaySound(MainWindow.SoundType.DefeatSound);
                        var close = OpenDialogWindow(DialogWindowType.DefeatWindow) ?? true;
                        if (!close)
                        {
                            BackToMenuClick();
                        }
                        else
                        {
                            _navigationService.Refresh();
                        }
                    }
                    break;
                case ResultType.IncorrectNewRecord:
                    {
                        _mainWindow?.PlaySound(MainWindow.SoundType.WinSound);
                        OpenDialogWindow(DialogWindowType.NewRecordWindow);
                        BackToMenuClick();
                    }
                    break;
            }
        }

        private ICommand _doAnswerButtonClick;

        public ICommand DoAnswerButtonClick
        {
            get
            {
                if (_doAnswerButtonClick == null)
                {
                    _doAnswerButtonClick = new Command(
                        p => true,
                        p => AnswerButtonClick(p));
                }
                return _doAnswerButtonClick;
            }
        }

        private ICommand _doButtonClickTime;

        public ICommand DoButtonClickTime
        {
            get
            {
                if (_doButtonClickTime == null)
                {
                    _doButtonClickTime = new Command(
                        p => true,
                        p => ButtonClickTime(p));
                }
                return _doButtonClickTime;
            }
        }

        private ICommand _doAssignHintsGrid;

        public ICommand DoAssignHintsGrid
        {
            get
            {
                if (_doAssignHintsGrid == null)
                {
                    _doAssignHintsGrid = new Command(
                        p => true,
                        p => AssignHintsGrid(p));
                }
                return _doAssignHintsGrid;
            }
        }

        private ICommand _doButtonClickStatistics;

        public ICommand DoButtonClickStatistics
        {
            get
            {
                if (_doButtonClickStatistics == null)
                {
                    _doButtonClickStatistics = new Command(
                        p => true,
                        p => ButtonClickStatistics());
                }
                return _doButtonClickStatistics;
            }
        }

        private ICommand _doButtonClickTwoAnswers;

        public ICommand DoButtonClickTwoAnswers
        {
            get
            {
                if (_doButtonClickTwoAnswers == null)
                {
                    _doButtonClickTwoAnswers = new Command(
                        p => true,
                        p => ButtonClickTwoAnswers());
                }
                return _doButtonClickTwoAnswers;
            }
        }

        private ICommand _doExitGamePageButtonClick;

        public ICommand DoExitGamePageButtonClick
        {
            get
            {
                if (_doExitGamePageButtonClick == null)
                {
                    _doExitGamePageButtonClick = new Command(
                        p => true,
                        p => ExitGamePageButtonClick());
                }
                return _doExitGamePageButtonClick;
            }
        }

        private ICommand _doOnTextBoxFocused;

        public ICommand DoOnTextBoxFocused
        {
            get
            {
                if (_doOnTextBoxFocused == null)
                {
                    _doOnTextBoxFocused = new Command(
                        p => true,
                        p => OnTextBoxFocused(p));
                }
                return _doOnTextBoxFocused;
            }
        }

        private ICommand _doPostPhotoClick;

        public ICommand DoPostPhotoClick
        {
            get
            {
                if (_doPostPhotoClick == null)
                {
                    _doPostPhotoClick = new Command(
                        p => true,
                        p => PostPhotoClick());
                }
                return _doPostPhotoClick;
            }
        }

        private ICommand _doSaveStatisticClick;

        public ICommand DoSaveStatisticClick
        {
            get
            {
                if (_doSaveStatisticClick == null)
                {
                    _doSaveStatisticClick = new Command(
                        p => true,
                        p => SaveStatisticClick());
                }
                return _doSaveStatisticClick;
            }
        }

        private ICommand _doSaveRecordClick;

        public ICommand DoSaveRecordClick
        {
            get
            {
                if (_doSaveRecordClick == null)
                {
                    _doSaveRecordClick = new Command(
                        p => true,
                        p => SaveRecordClick(p));
                }
                return _doSaveRecordClick;
            }
        }

        private ICommand _doOnMainMenuLoaded;

        public ICommand DoOnMainMenuLoaded
        {
            get
            {
                if (_doOnMainMenuLoaded == null)
                {
                    _doOnMainMenuLoaded = new Command(
                        p => true,
                        p => OnMainMenuLoaded(p));
                }
                return _doOnMainMenuLoaded;
            }
        }

        private ICommand _doBackToMenuClick;

        public ICommand DoBackToMenuClick
        {
            get
            {
                if (_doBackToMenuClick == null)
                {
                    _doBackToMenuClick = new Command(
                        p => true,
                        p => BackToMenuClick());
                }
                return _doBackToMenuClick;
            }
        }

        private ICommand _doHighscoreClick;

        public ICommand DoHighscoreClick
        {
            get
            {
                if (_doHighscoreClick == null)
                {
                    _doHighscoreClick = new Command(
                        p => true,
                        p => HighscoreClick());
                }
                return _doHighscoreClick;
            }
        }

        private ICommand _doLogOutClick;

        public ICommand DoLogOutClick
        {
            get
            {
                if (_doLogOutClick == null)
                {
                    _doLogOutClick = new Command(
                        p => true,
                        p => LogOutClick());
                }
                return _doLogOutClick;
            }
        }

        private ICommand _doInformationClick;

        public ICommand DoInformationClick
        {
            get
            {
                if (_doInformationClick == null)
                {
                    _doInformationClick = new Command(
                        p => true,
                        p => InformationClick());
                }
                return _doInformationClick;
            }
        }

        private ICommand _doCloseDialog;

        public ICommand DoCloseDialog
        {
            get
            {
                if (_doCloseDialog == null)
                {
                    _doCloseDialog = new Command(
                        p => true,
                        p => CloseDialog());
                }
                return _doCloseDialog;
            }
        }

        private ICommand _doStartNewGameClick;

        public ICommand DoStartNewGameClick
        {
            get
            {
                if (_doStartNewGameClick == null)
                {
                    _doStartNewGameClick = new Command(
                        p => true,
                        p => StartNewGameClick());
                }
                return _doStartNewGameClick;
            }
        }

        private ICommand _doReturnTrueResultClick;

        public ICommand DoReturnTrueResultClick
        {
            get
            {
                if (_doReturnTrueResultClick == null)
                {
                    _doReturnTrueResultClick = new Command(
                        p => true,
                        p => ReturnTrueResultClick());
                }
                return _doReturnTrueResultClick;
            }
        }

        private ICommand _doExitGamePageClick;

        public ICommand DoExitGamePageClick
        {
            get
            {
                if (_doExitGamePageClick == null)
                {
                    _doExitGamePageClick = new Command(
                        p => true,
                        p => ExitGamePageClick());
                }
                return _doExitGamePageClick;
            }
        }

        private ICommand _doStartNewGame;

        public ICommand DoStartNewGame
        {
            get
            {
                if (_doStartNewGame == null)
                {
                    _doStartNewGame = new Command(
                        p => true,
                        p => StartNewGame());
                }
                return _doStartNewGame;
            }
        }

        private ICommand _doReturnFalseResultClick;

        public ICommand DoReturnFalseResultClick
        {
            get
            {
                if (_doReturnFalseResultClick == null)
                {
                    _doReturnFalseResultClick = new Command(
                        p => true,
                        p => ReturnFalseResultClick());
                }
                return _doReturnFalseResultClick;
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