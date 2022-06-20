using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerConnector
{
    public class DateStringFormatter
    {
        public static string Format(DateTime date)
        {
            return date.ToString("yyyy-MM-ddTHH:mm:ss.ffff");
        }
    }
}
