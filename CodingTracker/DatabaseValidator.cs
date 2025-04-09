using System.Globalization;

namespace CodingTracker
{
    public static class DatabaseValidator
    {
        public static bool ValidateDateFormat(string date)
        {
            if (DateTime.TryParseExact(date, "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return true;
            }
            else
            {
                Console.WriteLine("Invalid date format. Please enter the date in the format 'MM-dd-yyyy HH:mm:ss'.");
                return false;
            }
        }

        //If the start time is before the end we have a problem
        public static bool ValidateStartIsBeforeEnd(string startTime, string endTime)
        {
            if(DateTime.Parse(startTime).Subtract(DateTime.Parse(endTime)) > TimeSpan.Zero)
            {
                Console.WriteLine("Start time must be before end time.");
                return false;
            }
            return true;
        }

        public static int GetQuantity(string message)
        {
            Console.WriteLine(message);

            string quantityInput = Console.ReadLine();

            if (quantityInput == "0") UserInputManager.Menu();

            while (!Int32.TryParse(quantityInput, out _) || Convert.ToInt32(quantityInput) < 0) //Handles negative numbers
            {
                Console.WriteLine("Invalid input. Please try again.");
                quantityInput = Console.ReadLine();
            }

            return Convert.ToInt32(quantityInput);
        }
    }
}
