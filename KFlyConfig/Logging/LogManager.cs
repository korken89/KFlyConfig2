using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFly.Logging
{
    public static class LogManager
    {
        public static Boolean Debug = false;

        private static ConcurrentStack<IKFlyLog> _logs = new ConcurrentStack<IKFlyLog>();

        public static void AddLogDestination(IKFlyLog log)
        {
            _logs.Push(log);
        }

        public static void LogInfoLine(String msg)
        {
            foreach (IKFlyLog log in _logs)
            {
                log.LogInfoLine(msg);
            }
        }

        public static void LogWarningLine(String msg)
        {
            foreach (IKFlyLog log in _logs)
            {
                log.LogWarningLine(msg);
            }
        }

        public static void LogErrorLine(String msg)
        {
            foreach (IKFlyLog log in _logs)
            {
                log.LogErrorLine(msg);
            }
        }

        public static void LogCriticalLine(String msg)
        {
            foreach (IKFlyLog log in _logs)
            {
                log.LogCriticalLine(msg);
            }
        }

        public static void LogDebugLine(String msg)
        {
            if (Debug)
            {
                foreach (IKFlyLog log in _logs)
                {
                    log.LogDebugLine(msg);
                }
            }
        }

        private static int _waitingLineCounter = 0;

        public static int LogWaitingLine(String msg)
        {
            return _waitingLineCounter;
        }
        

    }

    
}
