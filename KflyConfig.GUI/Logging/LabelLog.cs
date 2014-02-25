using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace KFly.Logging
{
    /// <summary>
    /// Shows latest message in a label
    /// </summary>
    public class LabelLog: IKFlyLog
    {
        private Label _label;
        public LabelLog(Label label)
        {
            _label = label;
        }

        private void LogLine(String msg, Color color)
        {
            _label.Dispatcher.BeginInvoke((Action)(() =>
            {
                _label.Content = msg;
                _label.Foreground = new SolidColorBrush(color);
            }));
         }
        public void LogInfoLine(String msg)
        {
            LogLine(msg, Colors.White);
        }

        public void LogErrorLine(String msg)
        {
            LogLine(msg, Colors.Red);
      
        }

        public void LogCriticalLine(String msg)
        {
            LogErrorLine(msg);
        }

        public void LogDebugLine(String msg)
        {
            //Dont show debug ever!   
        }

        public void LogWarningLine(String msg)
        {
            LogLine(msg, Colors.Yellow);
        }
    }
}
