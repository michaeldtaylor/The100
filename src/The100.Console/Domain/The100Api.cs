using System;

namespace The100.Console.Domain
{
    public static class The100Api
    {
        static readonly Uri BaseUri = new Uri("https://www.the100.io/");
        public static readonly Uri LoginUri = new Uri(BaseUri, "/login");
        public static readonly Uri GamingSessionsUri = new Uri(BaseUri, "/gaming_sessions/");
        public static readonly Uri NewGamingSessionUri = new Uri(BaseUri, "/gaming_sessions/new");

        public static Uri BuildUserUri(string userId)
        {
            return new Uri(BaseUri, $"/users/{userId}");
        }

        public static Uri BuildUserDashboardUri(string userId)
        {
            return new Uri(BaseUri, $"/users/{userId}/dashboard");
        }
    }
}