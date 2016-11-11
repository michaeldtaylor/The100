using System;

namespace The100.Console.Infrastructure
{
    public static class Password
    {
        const string The100PasswordKey = "The100Password";
        
        public static string Retrieve()
        {
            var password = Environment.GetEnvironmentVariable(The100PasswordKey);

            if (string.IsNullOrEmpty(password))
            {
                throw new Exception($"Please set the environmental variable '{The100PasswordKey}' and set your password.");
            }

            return password;
        }
    }
}