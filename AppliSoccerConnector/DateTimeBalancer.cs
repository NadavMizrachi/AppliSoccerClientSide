using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerConnector
{
    public class DateTimeBalancer
    {
        public static void BalanceToUtc(DateTime dateTime)
        {
            if(dateTime != null && dateTime.Kind != DateTimeKind.Utc)
            {
                dateTime.ToUniversalTime();
            }
        }
    }
}
