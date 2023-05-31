using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGIS.Core
{
    public static class LogHelper
    {
        private static log4net.ILog log;
        static LogHelper()
        {
            //if (ConfigHelper.GetAppConfig())
            log= log4net.LogManager.GetLogger("ErrorLogger");
        }
         
        public static void LogDebugMsg(string msg)
        {
            log.Debug(msg);
        }
        public static void LogInfoMsg(string msg)
        {
            log.Info(msg);
        }
        public static void LogErrorMsg(string msg)
        {
            log.Error(msg);
        }
    }
}




