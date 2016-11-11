using System;
using System.Collections.Generic;
using System.Linq;

namespace The100.Console.Domain
{
    public class SessionDefinition : SessionDefinitionBase
    {
        public DateTime StartTime { get; set; }
    }

    public abstract class SessionDefinitionBase
    {
        public SessionDefinitionBase()
        {
            FriendsToAdd = Enumerable.Empty<string>();
        }

        public string Activity { get; set; }
        public string Details { get; set; }
        public int GroupId { get; set; }
        public int LightLevel { get; set; }
        public IEnumerable<string> FriendsToAdd { get; set; }
    }
}