using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Data;


namespace KFly.GUI
{
    public class MainWindowTitleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            string res = "KFly Config - ";
            if ((values[0] is string) && ((values[0] as string) != ""))
            {
                res += (string)values[0];
            }
            else if (values[1] is string)
            {
                res += (string)values[1];
            }
          
            return  res;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return Array.ConvertAll<Type, Object>(targetTypes, t => value);
        }
    }
}
