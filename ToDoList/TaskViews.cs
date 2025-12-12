using System;
using System.Collections.Generic;
using System.Linq;

namespace ToDoList
{
    internal static class TaskViews
    {
        internal static void ShowTasks(TaskManager manager)
        {
            // Default view: show ALL tasks sorted by date (so nothing "disappears" when marked done)
            var currentView = manager.GetTasksSortedByDate();

            while (true)
            {
                Console.Clear();

                Console.WriteLine("Show Tasks\n");

                // show whatever the current view is
                PrintTaskList(currentView);

                Console.WriteLine();
                Console.WriteLine("View options:");
                FancyUi.ResetRainbow();
                FancyUi.WriteMenuOption("1", "Show TODO tasks sorted by date");
                FancyUi.WriteMenuOption("2", "Show ALL tasks sorted by date");
                FancyUi.WriteMenuOption("3", "Show ALL tasks sorted by project");
                FancyUi.WriteMenuOption("4", "Filter TODO tasks by project");
                Console.WriteLine("(B) Back");
                Console.Write("\nChoice: ");

                string? choice = Console.ReadLine();
                if (IsBack(choice)) return;

                switch (choice?.Trim().ToLowerInvariant())
                {
                    case "1":
                        currentView = manager.Tasks
                            .Where(task => !task.IsDone)
                            .OrderBy(task => task.DueDate)
                            .ToList();
                        break;

                    case "2":
                        currentView = manager.GetTasksSortedByDate();
                        break;

                    case "3":
                        currentView = manager.GetTasksSortedByProject();
                        break;

                    case "4":
                        Console.Write("\nProject name (or B to go back): ");
                        string? project = Console.ReadLine();
                        if (IsBack(project)) break;

                        currentView = manager.FilterByProject(project!)
                            .Where(task => !task.IsDone)
                            .OrderBy(task => task.DueDate)
                            .ToList();
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nInvalid choice. Try again.");
                        Console.ResetColor();
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        internal static void PrintTaskList(List<TodoTask> list)
        {
            if (list == null || list.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No tasks to show.");
                Console.ResetColor();
                return;
            }

            // Column widths 
            const int idWidth = 4;       
            const int statusWidth = 8;   
            const int projectWidth = 14;
            const int titleWidth = 28;  

            // Header (rainbow)
            WriteHeader(idWidth, statusWidth, projectWidth, titleWidth);
            Console.WriteLine();

            // Rows (white, done = gray)
            foreach (var task in list)
            {
                var old = Console.ForegroundColor;

                if (task.IsDone)
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                else
                    Console.ForegroundColor = ConsoleColor.White;

                string id = task.Id.ToString().PadRight(idWidth);
                string status = (task.IsDone ? "Done" : "Todo").PadRight(statusWidth);
                string project = TruncateWithDots(task.Project ?? "", projectWidth).PadRight(projectWidth); // trunk tr000nk
                string title = TruncateWithDots(task.Title ?? "", titleWidth).PadRight(titleWidth);
                string date = task.DueDate.ToString("yyyy-MM-dd");

                Console.WriteLine($"{id}{status}{project}{title}{date}");

                Console.ForegroundColor = old;
            }
        }

        private static void WriteHeader(int idWidth, int statusWidth, int projectWidth, int titleWidth)
        {
            // Make just the header words rainbow, not the rows
            FancyUi.ResetRainbow();
            WriteRainbowWord("ID".PadRight(idWidth));
            WriteRainbowWord("Status".PadRight(statusWidth));
            WriteRainbowWord("Project".PadRight(projectWidth));
            WriteRainbowWord("Title".PadRight(titleWidth));
            WriteRainbowWord("Due Date");
        }

        private static void WriteRainbowWord(string text)
        {
            foreach (char character in text)
            {
                var color = FancyUi.RainbowColors[_headerRainbowIndex % FancyUi.RainbowColors.Length];
                _headerRainbowIndex++;

                Console.ForegroundColor = color;
                Console.Write(character);
                Console.ResetColor();
            }
        }

        private static int _headerRainbowIndex = 0;

        private static string TruncateWithDots(string text, int width)
        {
            if (width <= 0) return "";
            if (string.IsNullOrEmpty(text)) return "";
            if (text.Length <= width) return text;
            if (width <= 3) return text.Substring(0, width);
            return text.Substring(0, width - 3) + "...";
        }

        private static bool IsBack(string? input)
        {
            return !string.IsNullOrWhiteSpace(input) &&
                   input.Trim().Equals("b", StringComparison.OrdinalIgnoreCase);
        }
    }
}
