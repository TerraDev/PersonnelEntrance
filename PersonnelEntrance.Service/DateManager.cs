using System;
using System.Collections.Generic;
using System.Text;

namespace PersonnelEntrance.Service
{
    internal static class DateManager
    {
        internal static DateTime ShortenDate(DateTime date)
        {
            date = date.AddMilliseconds(-date.Millisecond);
            date = date.AddSeconds(-date.Second);
            return date;
        }
    }
}
