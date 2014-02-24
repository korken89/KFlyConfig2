using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFly.Logging
{
    public interface IKFlyLog
    {
        void LogInfoLine(String msg);
        void LogErrorLine(String msg);
        void LogWarningLine(String msg);
        void LogCriticalLine(String msg);
    }
}
