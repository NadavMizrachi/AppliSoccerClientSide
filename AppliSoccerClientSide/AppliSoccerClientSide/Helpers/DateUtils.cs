using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerClientSide.Helpers
{
    public class DateUtils
    {
        public static DateTime ToUtc(DateTime dt)
        {
            if (dt == null) return DateTime.Now;
            return TimeZoneInfo.ConvertTimeToUtc(dt);
        }
        
        public static DateTime AddTimeSpanToDate(DateTime dt, TimeSpan ts)
        {
            if (dt == null || ts == null) return DateTime.Now;
            return dt.AddTicks(ts.Ticks);
        }
    }
}
