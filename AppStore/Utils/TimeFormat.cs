using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppStore.Utils
{
    public class TimeFormat
    {
        public static string toTaiwanTime(string milliseconds)
        {
            TimeSpan time = TimeSpan.FromMilliseconds(double.Parse(milliseconds));
            DateTime startdate = new DateTime(1970, 1, 1) + time;
            return startdate.AddHours(8).ToString("yyyy/MM/dd HH:mm:ss");
        }
    }
}