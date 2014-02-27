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
    public class RichTextBoxLog: IKFlyLog
    {
        private enum StyleType
        {
            Italic,
            Underline,
            Bold
        }

        private RichTextBox _box;
        public RichTextBoxLog(RichTextBox rtb)
        {
            _box = rtb;
        }

        private void DrawPart(Paragraph p, String text, HashSet<StyleType> styles, Color color)
        {
            if ((text != null) && (text.Length > 0))
            {
                var r = new Run();
                r.FontStyle = (styles.Contains(StyleType.Italic)) ? FontStyles.Italic : FontStyles.Normal;
                r.FontWeight = (styles.Contains(StyleType.Bold)) ? FontWeights.Bold : FontWeights.Normal;
                r.Foreground = new SolidColorBrush(color);
                r.Text = text;
                p.Inlines.Add(r);
            }
        }

        private dynamic DrawToParagraph(Paragraph p, String msg)
        {
            HashSet<StyleType> styles = new HashSet<StyleType>();
            Stack<Color> colors = new Stack<Color>();
            colors.Push(Colors.Black);

            Boolean endtag = false;
            int j = 0;
            String toAdd = "";
            for (int i = 0; i < msg.Length; i++)
            {
                switch (msg[i])
                {
                    case '<':
                        {
                            if (msg[i + 1] == '/')
                            {
                                endtag = true;
                                j = 1;
                            }
                            else
                            {
                                endtag = false;
                                j = 0;
                            }
                            if ((msg[i + j + 1] == 'b') && (msg[i + j + 2] == '>')) 
                            {
                                DrawPart(p, toAdd, styles, colors.Peek());
                                toAdd = "";
                                j += 2;
                                if (!endtag)
                                    styles.Add(StyleType.Bold);
                                else
                                    styles.Remove(StyleType.Bold);
                            }
                            else if ((msg[i + j + 1] == 'u') && (msg[i + j + 2] == '>'))
                            {
                                DrawPart(p, toAdd, styles, colors.Peek());
                                toAdd = "";
                                j += 2;
                                if (!endtag)
                                    styles.Add(StyleType.Underline);
                                else
                                    styles.Remove(StyleType.Underline);
                            }
                            else if ((msg[i + j + 1] == 'u') && (msg[i + j + 2] == '>'))
                            {
                                DrawPart(p, toAdd, styles, colors.Peek());
                                toAdd = "";
                                j += 2;
                                if (!endtag)
                                    styles.Add(StyleType.Italic);
                                else
                                    styles.Remove(StyleType.Italic);
                            }
                            else if (msg[i + j + 1] == '#')
                            {
                                DrawPart(p, toAdd, styles, colors.Peek());
                                toAdd = "";
                                j += 2;
                                if (!endtag)
                                {
                                    var k = 0;
                                    var temp = "#";
                                    while (msg[i + j + k] != '>')
                                    {
                                        temp += msg[i + j + k];
                                        k++;
                                    }
                                    j += k;
                                    if (temp.Length < 8)
                                        temp = "#FF" + temp.Substring(1, temp.Length-1);
                                    colors.Push((Color)ColorConverter.ConvertFromString(temp));
                                }
                                else
                                {
                                    colors.Pop();
                                }
                            }
                 
                        }
                        break;
                    default:
                        {
                            toAdd += msg[i];
                        }
                        break;
                }
                i += j;
                j = 0;
            }
            DrawPart(p, toAdd, styles, colors.Peek());
            if (_box.Document.Blocks.Count() > 30)
            {
                _box.Document.Blocks.Remove(_box.Document.Blocks.FirstBlock);
            }
             _box.Document.Blocks.Add(p);
            return p;
        }
        private void DrawHeader(Paragraph p)
        {
            var r = new Run();
            r.FontStyle = FontStyles.Italic;
            r.Text = "[" + DateTime.Now.ToLongTimeString() + "]: ";
            p.Inlines.Add(r);
        }
        public void LogInfoLine(String msg)
        {
            _box.Dispatcher.BeginInvoke((Action)(()=>{
                msg = "<b>" + msg + "</b>";
                Paragraph p = new Paragraph();
                DrawHeader(p);
                DrawToParagraph(p, msg);
            }));
        }

        public void LogErrorLine(String msg)
        {
            _box.Dispatcher.BeginInvoke((Action)(() =>
            {
                msg = "<b><#FF0000>" + msg + "</#></b>";
                Paragraph p = new Paragraph();
                DrawHeader(p);
                DrawToParagraph(p, msg);
            }));
        }

        public void LogDebugLine(String msg)
        {
            _box.Dispatcher.BeginInvoke((Action)(() =>
            {
                msg = "<b><#AA000000>" + msg + "</#></b>";
                Paragraph p = new Paragraph();
                DrawHeader(p);
                DrawToParagraph(p, msg);
            }));
        }

        public void LogCriticalLine(String msg)
        {
            LogErrorLine(msg);
        }

        public void LogWarningLine(String msg)
        {
            _box.Dispatcher.BeginInvoke((Action)(() =>
            {
                msg = "<b><#00FF00>" + msg + "</#></b>";
                Paragraph p = new Paragraph();
                DrawHeader(p);
                DrawToParagraph(p, msg);
            }));
        }
    }
}
