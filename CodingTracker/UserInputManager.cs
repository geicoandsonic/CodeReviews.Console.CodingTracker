using CodingTracker.UILayer;
namespace CodingTracker
{

    //This class is responsible for handling all the user input and for passing the data to the validator
    public static class UserInputManager
    {
        internal static bool isTimerOn;
        public static void Menu()
        {
            string? menuChoice;
            
            do
            {
                menuChoice = Display.PrintMainMenu();
                switch (menuChoice)
                {
                    case "Close Application":
                        Console.WriteLine("Exiting the application. Goodbye!");
                        Environment.Exit(0);
                        break;
                    case "View All Records":
                        RecordViewer.ViewRecords();
                        break;
                    case "Add Record":
                        DatabaseManager.AddRecord();
                        break;
                    case "Update Existing Record":
                        UpdateRecord();
                        break;
                    case "Delete Record":
                        DeleteRecord();
                        break;                  
                    case "Delete All Records":
                        DeleteAllRecords();
                        break;
                    case "Timer Status":
                        TimerStatus();
                        break;
                    case "Start Timer":
                        TimerMode();
                        break;
                    case "Stop Timer":
                        TimerMode();                        
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
                ReturnToMainMenu();
            } while (menuChoice != "Close App");            
        }

        public static void DeleteRecord()
        {
            Console.Clear();
            Display.PrintHeader("Delete Record", "red");
            if (!RecordViewer.ViewRecords(displayHeader:false))
            {
                return;
            }           
            int id = DatabaseValidator.GetQuantity("Enter the ID of the record you want to delete, or type 0 to return to the Main Menu.");

            DatabaseManager.DeleteRecord(id);           
        }

        

        internal static void DeleteAllRecords()
        {
            Console.Clear();
            Display.PrintHeader("Delete ALL Record", "red");
            Console.WriteLine("Are you sure you want to delete all records? This action is irreversible. (Y/N)");
            string choice = Console.ReadLine().ToUpper();
            if (choice == "Y")
            {
                DatabaseManager.DeleteAllRecords();
                Console.WriteLine("\nAll records cleared from the database.");
            }
            else
            {
                Menu();
            }
        }

        internal static void UpdateRecord()
        {
            Console.Clear();
            Display.PrintHeader("Update Record", "purple");
            if (!RecordViewer.ViewRecords(displayHeader:false))
            {
                return;
            }
            int id = DatabaseValidator.GetQuantity("Enter the ID of the record you want to update, or type 0 to return to the Main Menu.");

            DatabaseManager.UpdateRecord(id);
        }

        internal static void TimerMode()
        {           
            if(!isTimerOn)
            {
                Console.WriteLine("Starting timer. On the main menu, please select Stop Timer option to end the timer and your time will be recorded,");
                Console.WriteLine("or select Timer Status to see the current duration.");
                DatabaseManager.StartTimer();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Stopping timer.");
                int totalDuration = DatabaseManager.StopTimer();
                if(totalDuration != 1)
                {
                    Console.WriteLine($"Total time spent coding: {totalDuration} minutes.");
                }
                else
                {
                    Console.WriteLine($"Total time spent coding: {totalDuration} minute.");
                }
                
            }
            isTimerOn = !isTimerOn;            
        }

        internal static void ReturnToMainMenu()
        {
            Console.Write("\nPress any key to return to the Main Menu...");
            Console.ReadKey();
        }

        internal static void TimerStatus()
        {
            string startTime = DatabaseManager.GetStartTimeForTimer();
            string currentTime = DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss");
            int duration = (int)DateTime.Parse(currentTime).Subtract(DateTime.Parse(startTime)).TotalMinutes;

            if (duration != 1)
            {
                Console.WriteLine($"Total time spent coding: {duration} minutes.");
            }
            else
            {
                Console.WriteLine($"Total time spent coding: {duration} minute.");
            }
        }

        internal static bool GetTimerStatus()
        {
            return isTimerOn;
        }
    }
}
