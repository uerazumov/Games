using System;
using NLog;

namespace LoggingService
{
    public class GlobalLogger
    {
        private static GlobalLogger _instance;
        private Logger _logger;
        private static object _lock = new Object();

        public static GlobalLogger Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new GlobalLogger();
                        }
                    }
                }
                return _instance;
            }
        }

        private GlobalLogger()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Fatal(string message)
        {
            _logger.Fatal(message);
        }
    }
}
