using System;
using ManyConsole;
using The100.Console.Domain;
using The100.Console.Infrastructure;

namespace The100.Console.Commands
{
    public class CreateWeekSessionsCommand : ConsoleCommand
    {
        public CreateWeekSessionsCommand()
        {
            IsCommand("CreateWeekSessions", "Create 'The 100' sessions for the week.");

            HasRequiredOption("sfp|sessions-file-path=", "The full path of the week sessions file.", c => SessionFilePath = c);
        }

        public string SessionFilePath { get; set; }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                var weekSessionDefinition = SessionFileLoader.Load(SessionFilePath);
                var credentials = new Credentials(weekSessionDefinition.GamerTag, weekSessionDefinition.UserId, Password.Retrieve());
                var sessionCreator = new The100SessionCreator(credentials);

                var sessionResults = sessionCreator.CreateSessions(weekSessionDefinition);

                foreach (var sessionResult in sessionResults)
                {
                    System.Console.WriteLine($"Created a 'The 100' session with ID '{sessionResult.Id}' on {sessionResult.StartTime.ToLongDateString()}.");
                }
            }
            catch (Exception ex)
            {
                throw new ConsoleHelpAsException(ex.Message);
            }

            return 0;
        }
    }
}