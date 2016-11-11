using System.IO;
using Newtonsoft.Json;
using The100.Console.Domain;

namespace The100.Console.Infrastructure
{
    public static class SessionFileLoader
    {
        public static WeekSessionDefinition Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Could not find the week session file at '{filePath}'.", filePath);
            }

            var fileContents = File.ReadAllText(filePath);

            return JsonConvert.DeserializeObject<WeekSessionDefinition>(fileContents);
        }
    }
}