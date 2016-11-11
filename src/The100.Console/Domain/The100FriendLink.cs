using System;

namespace The100.Console.Domain
{
    internal class The100FriendLink
    {
        public The100FriendLink(string name, string href)
        {
            Name = name;
            Href = new Uri(href);
        }

        public string Name { get; }
        public Uri Href { get; }
    }
}