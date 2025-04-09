using CodingTracker.UILayer;
using Microsoft.Data.Sqlite;
namespace CodingTracker
{
    public static class DatabaseManager
    {
        private static string _connectionString = "";
        private static string _databaseName = "";
        private static DateTime startTime;
        public static void CreateDatabase(string connectionString, string databaseName)
        {
            _connectionString = connectionString;
            _databaseName = databaseName;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open(); //Open database
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @$"CREATE TABLE IF NOT EXISTS {_databaseName} (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartTime TEXT,
                        EndTime TEXT,
                        Duration INTEGER)";

                tableCmd.ExecuteNonQuery();
            }
        }
        public static string GetTime()
        {
            string time = "";
            while (true)
            {
                Console.WriteLine("Format (MM-dd-yyyy HH:mm:ss): ");
                time = Console.ReadLine();
                if (DatabaseValidator.ValidateDateFormat(time))
                {
                    break;
                }
            }
            return time;
        }

        public static void AddRecord()
        {
            Console.Clear();
            Display.PrintHeader("Add Record");
            string startTime, endTime = "";
            Console.WriteLine("\nPlease enter the time you began coding.");
            startTime = GetTime();
            Console.WriteLine("\nPlease enter the time you finished coding.");
            endTime = GetTime();
            while (true)
            {
                if (DatabaseValidator.ValidateStartIsBeforeEnd(startTime, endTime))
                {
                    break;
                }
                Console.WriteLine("\nPlease enter the time you finished coding.");
                endTime = GetTime();
            }
            Console.Clear();
            Console.WriteLine($"Entry added successfully.");
            
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open(); //Open database
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"INSERT INTO {_databaseName} (StartTime,EndTime,Duration) VALUES('{startTime}','{endTime}'" +
                    $",{(int)DateTime.Parse(endTime).Subtract(DateTime.Parse(startTime)).TotalMinutes})";

                tableCmd.ExecuteNonQuery();

                connection.Close(); //Close database
            }
        }

        public static void DeleteRecord(int id)
        {
            Console.Clear();
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open(); //Open database
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"DELETE FROM {_databaseName} WHERE Id = '{id}'";

                int rowCount = tableCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\nNo record found with the given ID {id}");
                }
                else
                {
                    Console.WriteLine($"\nEntry {id} deleted successfully.");
                }
                connection.Close(); //Close database
            }
        }

        public static void DeleteAllRecords()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open(); //Open database
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"TRUNCATE {_databaseName}";

                tableCmd.ExecuteNonQuery();

                connection.Close(); //Close database
            }
        }

        public static bool UpdateRecord(int id)
        {
            Console.Clear();
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open(); //Open database
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"SELECT * FROM {_databaseName} WHERE Id = {id}";

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    Console.WriteLine($"\nNo record found with the given ID {id}");
                    connection.Close(); //Close database
                    return false;
                }
                else
                {         
                    reader.Close();
                    RecordViewer.ViewRecords(id);
                    Console.WriteLine("\nPlease enter the new start time.");
                    string startTime = GetTime();
                    Console.WriteLine("\nPlease enter the new end time.");
                    string endTime = GetTime();
                    while (true)
                    {
                        if (DatabaseValidator.ValidateStartIsBeforeEnd(startTime, endTime))
                        {
                            break;
                        }
                        Console.WriteLine("\nPlease enter the time you finished coding.");
                        endTime = GetTime();
                    }
                    Console.Clear();
                    Console.WriteLine($"Entry updated successfully.");

                    tableCmd.CommandText =
                        $"UPDATE {_databaseName} " +
                        $"SET StartTime = '{startTime}'," +
                        $"EndTime = '{endTime}'," +
                        $"Duration = {(int)DateTime.Parse(endTime).Subtract(DateTime.Parse(startTime)).TotalMinutes} " +
                        $"WHERE Id = {id}";

                    tableCmd.ExecuteNonQuery();

                    connection.Close(); //Close database
                    
                    return true;
                }               
            }
        }

        public static void StartTimer()
        {
            startTime = DateTime.Now;
        }

        public static int StopTimer()
        {
            string startTimeString = startTime.ToString("MM-dd-yyyy HH:mm:ss");
            string endTimeString = DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss");
            int duration = (int)DateTime.Parse(endTimeString).Subtract(DateTime.Parse(startTimeString)).TotalMinutes;
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open(); //Open database
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"INSERT INTO {_databaseName} (StartTime,EndTime,Duration) VALUES('{startTimeString}','{endTimeString}'" +
                    $",{duration})";

                tableCmd.ExecuteNonQuery();

                connection.Close(); //Close database
            }
            return duration;
        }

        public static string GetStartTimeForTimer()
        {
            return startTime.ToString("MM-dd-yyyy HH:mm:ss");
        }
        
    }
}
