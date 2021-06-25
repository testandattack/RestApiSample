using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;

namespace Extensions
{
    public static class DateTimeExtensions
    {
        public static int DaysSince2000(this DateTime theDate)
        {
            return (int)theDate.Subtract(new DateTime(2000, 1, 1)).TotalDays;
        }

        public static double GetElapsedSeconds(this DateTime dateTime)
        {
            TimeSpan ts = DateTime.UtcNow - dateTime;
            return ts.TotalSeconds;
        }

        public static string GetElapsedSecondsForDisplay(this DateTime dateTime)
        {
            return dateTime.GetElapsedSeconds().ToString("N1");
        }
    }
}
