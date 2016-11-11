using System;

namespace The100.Console.Domain
{
    public class DaySessionDefinition : SessionDefinitionBase
    {
        public DayOfWeek DayOfWeek { get; set; }
        public int Hour { get; set; }
        public int Minutes { get; set; }

        public SessionDefinition Build(DateTime weekStart)
        {
            if (Hour < 0 || Hour > 23)
            {
                throw new ArgumentException("Hour is invalid.", nameof(Hour));
            }

            if (Minutes < 0 || Minutes > 59)
            {
                throw new ArgumentException("Minutes is invalid.", nameof(Minutes));
            }

            var sessionDefinition = new SessionDefinition
            {
                Activity = Activity,
                Details = Details,
                GroupId = GroupId,
                LightLevel = LightLevel,
                FriendsToAdd = FriendsToAdd,
            };

            var daysUntilDayOfWeek = ((int)DayOfWeek - (int)weekStart.DayOfWeek + 7) % 7;
            var sessionStartDate = weekStart.AddDays(daysUntilDayOfWeek);

            sessionDefinition.StartTime = new DateTime(sessionStartDate.Year, sessionStartDate.Month, sessionStartDate.Day, Hour, Minutes, 0);

            return sessionDefinition;
        }
    }
}