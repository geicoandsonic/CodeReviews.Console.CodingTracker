using CodingTracker.UILayer;
using Microsoft.Data.Sqlite;
using System.Configuration;
using System.Globalization;

//This class is responsible for displaying the records to the user.
namespace CodingTracker
{
    public static class RecordViewer
    {
        static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
        static string databaseName = ConfigurationManager.AppSettings.Get("DatabaseName");
        public static bool ViewRecords(int specificRecord = -1, bool displayHeader = true)
        {          
            if (displayHeader)
            {
                Console.Clear();
                Display.PrintHeader("View Records", "green");
            }            
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open(); //Open database
                var tableCmd = connection.CreateCommand();
                if(specificRecord >= 0)
                {
                    tableCmd.CommandText =
                    $"SELECT * FROM {databaseName} WHERE Id = '{specificRecord}'";
                }
                else
                {
                    tableCmd.CommandText =
                    $"SELECT * FROM {databaseName}";
                }
                

                List<CodingSession> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                        new CodingSession
                        {
                            Id = reader.GetInt32(0),
                            StartTime = DateTime.ParseExact(reader.GetString(1), "MM-dd-yyyy HH:mm:ss", new CultureInfo("en-US")).ToString("MM-dd-yyyy HH:mm:ss"),
                            EndTime = DateTime.ParseExact(reader.GetString(2), "MM-dd-yyyy HH:mm:ss", new CultureInfo("en-US")).ToString("MM-dd-yyyy HH:mm:ss"),
                            Duration = reader.GetInt32(3)
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No records found.");
                    connection.Close();
                    return false;
                }

                connection.Close(); //Close database

                Console.WriteLine("----------------------------------------------\n");
                foreach (var row in tableData)
                {
                    Console.WriteLine($"ID: {row.Id} | Start: {row.StartTime} | End: {row.EndTime} | Duration (minutes): {row.Duration}");
                }
            }
            return true;
        }
    }
}
