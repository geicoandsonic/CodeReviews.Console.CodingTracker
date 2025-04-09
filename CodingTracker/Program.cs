using System.Configuration;
using System.Collections.Specialized;

namespace CodingTracker
{
    internal class Program
    {
        static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
        static string databaseName = ConfigurationManager.AppSettings.Get("DatabaseName");
        static void Main(string[] args)
        {
            DatabaseManager.CreateDatabase(connectionString,databaseName);

            UserInputManager.Menu();
        }
    }
}