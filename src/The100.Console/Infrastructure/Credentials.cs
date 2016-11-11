namespace The100.Console.Infrastructure
{
    public class Credentials
    {
        public Credentials(string gamerTag, string userId, string password)
        {
            GamerTag = gamerTag;
            UserId = userId;
            Password = password;
        }

        public string GamerTag { get; }
        public string UserId { get; }
        public string Password { get; }
    }
}