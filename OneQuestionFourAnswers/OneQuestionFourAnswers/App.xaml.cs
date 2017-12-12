using System;
using System.Windows;

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
                MessageBox.Show("К сожалению что-то пошло не так, сообщите разработчику о данной ошибке. Приносим вам наши извинения!" + exception.Message, "Uncaught Thread Exception",MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
            {

            }
            finally
            {
                Environment.Exit(0);
            }
        }
    }
}
