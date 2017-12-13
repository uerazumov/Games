using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace LoggingService
{
    public class Logger
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
    }
}
