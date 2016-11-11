using System;
using System.Collections.Generic;
using System.Linq;
using The100.Console.Infrastructure;

namespace The100.Console.Domain
{
    public class The100SessionCreator
    {
        readonly Credentials _credentials;

        public The100SessionCreator(Credentials credentials)
        {
            _credentials = credentials;
        }

        public IEnumerable<SessionResult> CreateSessions(IEnumerable<SessionDefinition> sessionDefinitions)
        {
            return (from sessionDefinition in sessionDefinitions
                    let session = new The100Session(_credentials)
                    select session.Create(sessionDefinition)).ToList();
        }

        public IEnumerable<SessionResult> CreateSessions(WeekSessionDefinition weekSessionDefinition)
        {
            var sessionDefinitions = weekSessionDefinition.Sessions.Select(d => d.Build(weekSessionDefinition.WeekStart ?? DateTime.Today)).ToList();

            return CreateSessions(sessionDefinitions);
        }
    }
}