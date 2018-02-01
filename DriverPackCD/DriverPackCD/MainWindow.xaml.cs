using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace DriverPackCD
{
    public partial class MainWindow : Window
    {
        private Process _InfoApp;
        private Process _LANApp;
        private Process _OnlineApp;

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public void Close(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void LAN(object sender, RoutedEventArgs e)
        {
            string ExecutableFilePath = @"LAN\DriverPackSolution.exe";
            try
            {
                _LANApp = new Process();
                _LANApp.StartInfo.FileName = ExecutableFilePath;
                _LANApp.Start();
            }
            catch { ShowNotify(); }
            LANButton.IsEnabled = false;
            try { CheckIsAppExist(LANButton, _LANApp); } catch { LANButton.IsEnabled = true; }
        }

        private void Online(object sender, RoutedEventArgs e)
        {
            string ExecutableFilePath = "DriverPack-17-Online.exe";
            try
            {
                _OnlineApp = new Process();
                _OnlineApp.StartInfo.FileName = ExecutableFilePath;
                _OnlineApp.Start();
            }
            catch { ShowNotify(); }
            OnlineButton.IsEnabled = false;
            try { CheckIsAppExist(OnlineButton, _OnlineApp); } catch { OnlineButton.IsEnabled = true; }
        }

        private void Info(object sender, RoutedEventArgs e)
        {
            string ExecutableFilePath = @"VisualResources\Info.txt";
            try
            {
                _InfoApp = new Process();
                _InfoApp.StartInfo.FileName = ExecutableFilePath;
                _InfoApp.Start();
            }
            catch { ShowNotify(); }
            InfoButton.IsEnabled = false;
            try { CheckIsAppExist(InfoButton, _InfoApp); } catch { InfoButton.IsEnabled = true; }
        }

        private void ShowNotify()
        {
            Notify n = new Notify();
            n.Owner = this;
            n.ShowDialog();
        }

        private void CheckIsAppExist(UserButton button, Process app)
        {
            new Thread(() =>
            {
                try {
                    int i = 0;
                    while (i != 1)
                        if (app.HasExited)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                button.IsEnabled = true;
                            });
                            i++;
                        }
                }
                catch
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        button.IsEnabled = true;
                    });
                }
            }).Start();
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            CloseButton.ControlButton.Click += Close;
            LANButton.Button.Content = "Driver Pack LAN";
            LANButton.Button.Click += LAN;
            OnlineButton.Button.Content = "Driver Pack Online";
            OnlineButton.Button.Click += Online;
            InfoButton.Button.Content = "Инструкция по установке драйверов";
            InfoButton.Button.FontSize = 20;
            InfoButton.Button.Click += Info;
        }
    }
}
