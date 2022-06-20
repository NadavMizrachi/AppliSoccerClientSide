using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerClientSide.Helpers.Mocking
{
    public class RandomDate
    {
        private DateTime _start;
        private DateTime _end;
        private Random _rnd;
        public RandomDate(DateTime start, DateTime end)
        {
            _start = start;
            _end = end; 
        }

        public DateTime Generate()
        {
            int range = (_end - _start).Days;
            return _start.AddDays(_rnd.Next(range));
        }
    }
}
