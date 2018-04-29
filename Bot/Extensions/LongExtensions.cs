using System;
using System.Collections.Generic;
using System.Text;

namespace OWLeagueBot.Extensions
{
    public static class LongExtensions
    {
        public static DateTime ConvertLongIntoDateTime(this long? number)
        {
            if (number != null)
            {
                var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var date = start.AddMilliseconds((long)number);
                return date;
            }
            else return DateTime.UtcNow;
        }
    }
}
