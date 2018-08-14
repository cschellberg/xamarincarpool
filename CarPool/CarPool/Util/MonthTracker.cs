using System;
using System.Collections.Generic;
using System.Text;

namespace Util
{
    public class MonthTracker
    {
        private DateTime dateTime;

        public MonthTracker()
        {
            dateTime = DateTime.Now;
        }

        public String GetMonthYearStr()
        {
            return dateTime.ToString("MMMM")+", "+dateTime.Year;
        }

        public int GetMonthNumber()
        {
            return dateTime.Month;
        }
        
        public int GetYearNumber()
        {
            return dateTime.Year;
        }

        public void IncrementMonth()
        {
            dateTime=dateTime.AddMonths(1);
        }

        public void DecrementMonth()
        {
            dateTime=dateTime.AddMonths(-1);
        }
    }
}
