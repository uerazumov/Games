using System;
using System.Windows;
using LoggingService;

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
            try
            {
                Exception exception = (Exception)args.ExceptionObject;
                GlobalLogger.Instance.Fatal("Приложение было завершено с критической ошибкой " + exception.Message);
                MessageBox.Show("К сожалению что-то пошло не так, сообщите разработчику о данной ошибке. Приносим вам наши извинения! " + exception.Message, "Uncaught Thread Exception",MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
            {

            }
            finally
            {
                //REVIEW: 0 - означает выход без ошибки. Если идёт выход с ошибкой - то выход должен быть с каким-нибудь кодом
                Environment.Exit(0);
            }
        }
    }
}
