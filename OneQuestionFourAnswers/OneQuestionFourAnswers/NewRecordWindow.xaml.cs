using LoggingService;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace OneQuestionFourAnswers
{
    public partial class NewRecordWindow
    {
        private TextBox _textBox;
        private readonly MainWindowViewModel _vm;
        private byte CountOfAwaitings = 0;

        public NewRecordWindow()
        {
            InitializeComponent();
            _vm = (MainWindowViewModel)Application.Current.Resources["MainWindowVM"];
            //REVIEW: А отписываться как будем от этого события?
            Loaded += (obj, args) =>
            {
                UsernameTextBox.Focus();
                UsernameTextBox.SelectAll();
                _textBox = BorderUsernameTextBox.Child as TextBox;
                (_textBox).GotFocus += OnTextBoxFocused;
            };
        }
        //REVIEW: MVVM пошёл и повесился... Что за code-behind?
        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            if (!Validation.GetHasError(_textBox) && ((_textBox).Text != ""))
            {
                new Thread(() =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MakeButtonDisabled(CloseButton);
                        MakeButtonDisabled(SaveButton);
                        WaitMessage.Visibility = Visibility.Visible;
                        CountOfAwaitings++;
                    });
                    if (_vm.CreateNewRecord())
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            BorderUsernameTextBox.IsEnabled = false;
                            NoticeTextBlock.Visibility = Visibility.Collapsed;
                            RecordError.Visibility = Visibility.Collapsed;
                            RecotdSuccessfully.Visibility = Visibility.Visible;
                        });
                    }
                    else
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            MakeButtonEnabled(SaveButton);
                            NoticeTextBlock.Visibility = Visibility.Collapsed;
                            RecordError.Visibility = Visibility.Visible;
                        });
                    };
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (CountOfAwaitings-1 == 0)
                        {
                            WaitMessage.Visibility = Visibility.Collapsed;
                            MakeButtonEnabled(CloseButton);
                        }
                        CountOfAwaitings--;
                    });
                }).Start();
            }
            else
            {
                RecordError.Visibility = Visibility.Collapsed;
                NoticeTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void OnTextBoxFocused(object sender, RoutedEventArgs e)
        {
            //REVIEW: Опять же, MVVM
            if (!Validation.GetHasError(_textBox))
            {
                return;
            }
            (_textBox).Text = "";
            var property = TextBox.TextProperty;
            Debug.Assert(property != null, "TextBox.TextProperty != null");
            Validation.ClearInvalid(_textBox.GetBindingExpression(property));
            NoticeTextBlock.Visibility = Visibility.Collapsed;
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //REVIEW: В команду!
        private void SaveStatisticButtonClick(object sender, RoutedEventArgs e)
        {
            new Thread(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MakeButtonDisabled(CloseButton);
                    MakeButtonDisabled(SaveStatistic);
                    WaitMessage.Visibility = Visibility.Visible;
                    CountOfAwaitings++;
                });
                var result = _vm.CreateReport();
                if (result == true)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ReportError.Visibility = Visibility.Collapsed;
                        ReportSuccessfully.Visibility = Visibility.Visible;
                    }); 
                }
                else if (result == false)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MakeButtonEnabled(SaveStatistic);
                        ReportError.Visibility = Visibility.Visible;
                    });
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (CountOfAwaitings - 1 == 0)
                    {
                        WaitMessage.Visibility = Visibility.Collapsed;
                        MakeButtonEnabled(CloseButton);
                    }
                    CountOfAwaitings--;
                });
            }).Start();
        }

        //REVIEW: Это делается через свойства, конвертер и биндинг
        private void MakeButtonDisabled(Button button)
        {
            button.IsEnabled = false;
            button.Foreground = Brushes.Gray;
        }
        //REVIEW: И это - тоже
        private void MakeButtonEnabled(Button button)
        {
            button.IsEnabled = true;
            button.Foreground = Brushes.White;
        }

        //REVIEW: В команду убрать
        private void SaveRecordIntoVKButtonClick(object sender, RoutedEventArgs e)
        {
            new Thread(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MakeButtonDisabled(CloseButton);
                    MakeButtonDisabled(SaveRecordIntoVK);
                    WaitMessage.Visibility = Visibility.Visible;
                    CountOfAwaitings++;
                });
                bool? webResult = true;
                if (!_vm.IsTokenExist())
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        WebBrowser web = new WebBrowser();
                        web.ShowDialog();
                        web.Owner = Owner;
                        webResult = web.DialogResult;
                    });
                }
                if (webResult != null)
                {
                    if ((webResult ?? false) && _vm.CreateRec())
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            VkError.Visibility = Visibility.Collapsed;
                            VkSuccessfully.Visibility = Visibility.Visible;
                        });
                    }
                    else
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            MakeButtonEnabled(SaveRecordIntoVK);
                            VkError.Visibility = Visibility.Visible;
                        });
                    }
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (CountOfAwaitings - 1 == 0)
                    {
                        WaitMessage.Visibility = Visibility.Collapsed;
                        MakeButtonEnabled(CloseButton);
                    }
                    CountOfAwaitings--;
                });
            }).Start();
        }
        
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = ((FrameworkElement)Resources["KinectCursor"]).Cursor;
        }
    }
}
