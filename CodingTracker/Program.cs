namespace CodingTracker
{

    internal class Program
    {
        static string connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString");
        static string databaseName = System.Configuration.ConfigurationManager.AppSettings.Get("DatabaseName");
        static void Main(string[] args)
        {
            DatabaseManager.CreateDatabase(connectionString,databaseName);

            UserInputManager.Menu();
        }
    }
}