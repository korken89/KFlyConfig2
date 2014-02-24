using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

namespace KFly.Logging
{
    public class RichTextBoxLog: IKFlyLog
    {
        private RichTextBox _box;
        public RichTextBoxLog(RichTextBox rtb)
        {
            _box = rtb;
        }

        public void WriteLine(String msg)
        {
        //    Paragraph p = new Paragraph();
         //   _box.
           
        }
    }
}
