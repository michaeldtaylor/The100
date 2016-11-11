using System;
using System.Collections.Generic;

namespace The100.Console.Domain
{
    public class WeekSessionDefinition
    {
        public WeekSessionDefinition()
        {
            Sessions = new List<DaySessionDefinition>();
        }

        public string GamerTag { get; set; }
        public string UserId { get; set; }
        public DateTime? WeekStart { get; set; }
        public IEnumerable<DaySessionDefinition> Sessions { get; set; }
    }
}