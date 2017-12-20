using System;
using System.Windows;
using LoggingService;
using System.Threading.Tasks;

namespace OneQuestionFourAnswers
{
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(GlobalExceptionHandler);
        }

        private static void GlobalExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            int exitCode = 0;
            try
            {
                Exception exception = (Exception)args.ExceptionObject;
                exitCode = exception.GetHashCode();
                GlobalLogger.Instance.Fatal("Приложение было завершено с критической ошибкой " + exception.ToString());
                MessageBox.Show("К сожалению что-то пошло не так, сообщите разработчику о данной ошибке. Приносим вам наши извинения! " + exception.Message, "Uncaught Thread Exception",MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
            {
                exitCode = -1;
            }
            finally
            {
                Environment.Exit(exitCode);
            }
        }
    }
}
