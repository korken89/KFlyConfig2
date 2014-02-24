using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFly.Logging
{
    public static class LogManager
    {
        private static List<IKFlyLog> _logs = new List<IKFlyLog>();

        public static void AddLogDestination(IKFlyLog log)
        {
            _logs.Add(log);
        }

        public static void LogInfoLine(String msg)
        {
        }

        public static void LogWarningLine(String msg)
        {
        }

        public static void LogErrorLine(String msg)
        {
        }

        public static void LogCriticalLine(String msg)
        {
        }

        private static int _waitingLineCounter = 0;

        public static int LogWaitingLine(String msg)
        {
            return _waitingLineCounter;
        }
        

    }

    public interface IKFlyLog
    {
        void WriteLine(String msg);
       // void WriteLine(String msg, int id);
      //  void AppendLine(int id, String appendText);
    }
}
