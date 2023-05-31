using OGIS.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OGIS.Core
{
    public class WriteLog
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(WriteLog));
        private static WriteLog _instance;
        public static WriteLog Instance
        {
            get { return _instance ?? (_instance = new WriteLog()); }
        }
        public void WriteErrMsg(string msg, Exception ex, LogType type = LogType.Error)
        {
            if (type == LogType.ErrShow)
                LogHelper.LogInfoMsg(string.Format("{0}  异常：{1}", msg, ex.Message));
            else
                LogHelper.LogInfoMsg(string.Format("{0}  异常：{1}", msg, ex.ToString()));
        }
        public void WriteMsg(LogType type, string msg)
        {
            if (type == LogType.Info)
                MessageBox.Show(msg);
            else if (type == LogType.Error)
                LogHelper.LogInfoMsg(msg);
            else if (type == LogType.ErrShow)
            {
                LogHelper.LogInfoMsg(msg);
                MessageBox.Show(msg);
            }
            else if (type == LogType.Warn)
            {
                MessageBox.Show(msg);
            }
            else if (type == LogType.File)
            {
                LogHelper.LogInfoMsg(msg);
            }
        }

        /// <summary>
        /// 记录信息到文件日志
        /// </summary>
        /// <param name="msg"></param>
        public void WriteInfoLogs(string msg)
        {
            LogHelper.LogInfoMsg(msg);
        }

        public void Worn(string msg)
        {
            MessageBox.Show(msg);
        }

    }
}
