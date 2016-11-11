using System;

namespace The100.Console.Domain
{
    public class SessionResult
    {
        public SessionResult(string id, string details, DateTime startTime)
        {
            Id = id;
            Details = details;
            StartTime = startTime;
        }

        public string Id { get; }
        public string Details { get; }
        public DateTime StartTime { get; }
    }
}