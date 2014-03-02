using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFly
{
    public static class DateExtension
    {
        public static Int32 ToUnixTimestamp(this DateTime target)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, target.Kind);
            var unixTimestamp = System.Convert.ToInt32((target - date).TotalSeconds);

            return unixTimestamp;
        }

        public static DateTime ToDateTime(this Int32 timestamp)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0);

            return dateTime.AddSeconds(timestamp);
        }
    }
}
