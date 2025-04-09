using Spectre.Console;

namespace CodingTracker.UILayer
{
    public static class Display
    {
        public static string PrintMainMenu()
        {
            Console.Clear();
            string menuChoice;
            if(!UserInputManager.GetTimerStatus())
            {
                menuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("MAIN MENU")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "Close Application", "View All Records", "Add Record", "Update Existing Record", "Delete Record", "Delete All Records", "Start Timer"
                }));
            }
            else
            {
                menuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("MAIN MENU")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "Close Application", "View All Records", "Add Record", "Update Existing Record", "Delete Record", "Delete All Records", "Timer Status", "Stop Timer"
                }));
            }

            return menuChoice;
        }

        public static void PrintHeader(string headerInput, string color = "blue")
        {
            // Print a simple heading
            var header = new Rule($"[{color}]{headerInput}[/]");
            header.Justification = Justify.Left;
            AnsiConsole.Write(header);
        }
    }
}
