using System;
using System.Linq;
using System.Collections.Generic;

namespace ToDoList
{
    internal class Program
    {
        // This is the file where tasks are stored between runs.
        // Visual Studio puts it in: bin/debug/net10.0/ (different tho depending on your .NET) :) 
        private const string FilePath = "tasks.txt";

        static void Main()
        {
            var taskmanager = new TaskManager();
            taskmanager.LoadFromFile(FilePath);

            // Main menu loop
            while (true)
            {
                Console.Clear();

                // Fancy ASCII banner + snow \o/
                FancyUi.WriteBanner();

                // A tiny dashboard so the user knows how productive they are (or not lol).
                int todo = taskmanager.Tasks.Count(task => !task.IsDone);
                int done = taskmanager.Tasks.Count(task => task.IsDone);

                Console.WriteLine($"You have {todo} tasks to do and {done} are done.");

                // Tiny progress bar so user can see how pro they are
                FancyUi.PrintProgressBar(todo, done);

                Console.WriteLine("Pick an option:");
                FancyUi.ResetRainbow();
                FancyUi.WriteMenuOption("1", "Show Task List (by date or project)");
                FancyUi.WriteMenuOption("2", "Add New Task");
                FancyUi.WriteMenuOption("3", "Edit Task (update, mark done, remove)");
                FancyUi.WriteMenuOption("4", "Save and Quit");
                Console.Write("\nChoice: ");

                string? userchoice = Console.ReadLine();

                switch (userchoice)
                {
                    case "1":
                        ShowTasks(taskmanager);
                        break;
                    case "2":
                        AddTask(taskmanager);
                        break;
                    case "3":
                        EditMenu(taskmanager);
                        break;
                    case "4":
                        // This is the magic moment where everything is written to the file.
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nSaving...");
                        taskmanager.SaveToFile(FilePath);
                        Console.WriteLine("Saved. Bye!");
                        Console.ResetColor();
                        return;

                    default:
                        // if the user chooses the wrong stuff, you let them know :D
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid choice. Please try again!");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Show tasks, with sorting and filtering options
        private static void ShowTasks(TaskManager manager)
        {
            // Start view: only TODO tasks, sorted by date
            var currentView = manager.Tasks
                .Where(task => !task.IsDone)
                .OrderBy(task => task.DueDate)
                .ToList();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Show Tasks");

                // show whatever the current view is
                PrintTaskTable(currentView);

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

        // Add a new task, with back support and no past dates, and loop for "add another"
        private static void AddTask(TaskManager manager)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Add New Task (B = Back)\n");

                // TITLE — required, so we nag gently until the user gives us something valid.
                Console.Write("Title: ");
                string? title = Console.ReadLine();
                if (IsBack(title)) return;

                // Keep asking until we get a valid title
                while (string.IsNullOrWhiteSpace(title))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Title cannot be empty. Enter title or B to go back: ");
                    Console.ResetColor();
                    title = Console.ReadLine();
                    if (IsBack(title)) return;
                }

                // Project (optional, but B still works)
                Console.Write("Project: ");
                string? project = Console.ReadLine();
                if (IsBack(project)) return;

                // DUE DATE — must be today or later because… well… todo list logic :)
                // added so that it accepts today and tomorrow as choice too, easier to test :D
                DateTime dueDate;
                while (true)
                {
                    Console.Write("Due date (yyyy-MM-dd, yyyyMMdd, 'today' or 'tomorrow', B = back): ");
                    string? input = Console.ReadLine();
                    if (IsBack(input)) return;

                    if (!TryParseFlexibleDate(input, out dueDate))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid date format.");
                        Console.ResetColor();
                        continue;
                    }

                    if (dueDate.Date < DateTime.Today)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Cannot add a task in the past. (Why isn't it done already?)");
                        Console.ResetColor();
                        continue;
                    }

                    break;
                }

                // here task is created through TaskManagerr
                manager.AddTask(title!, project ?? string.Empty, dueDate);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nTask added successfully!");
                Console.ResetColor();

                // ask if they want to add another
                while (true)
                {
                    Console.Write("\nAdd another task? (Y = yes, B = back): ");
                    string? again = Console.ReadLine();

                    if (IsBack(again)) return;

                    if (!string.IsNullOrWhiteSpace(again) &&
                        again.Trim().Equals("y", StringComparison.OrdinalIgnoreCase))
                    {
                        // inner loop breaks, outer while adds another
                        break;
                    }

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid choice.");
                    Console.ResetColor();
                }
            }
        }

        // Menu for editing an existing task
        // this could be split into smaller methods, but it is what it is.
        private static void EditMenu(TaskManager manager)
        {
            // show all tasks once (sorted by date), reuse same table style
            Console.Clear();
            Console.WriteLine("All tasks");
            var all = manager.GetTasksSortedByDate();
            PrintTaskTable(all);

            Console.Write("\nEnter ID to edit (B = back): ");
            string? idInput = Console.ReadLine();
            if (IsBack(idInput)) return;
            if (!int.TryParse(idInput, out int id)) return;

            Console.WriteLine("\nEdit options:");
            FancyUi.ResetRainbow();
            FancyUi.WriteMenuOption("1", "Update task");
            FancyUi.WriteMenuOption("2", "Mark as done");
            FancyUi.WriteMenuOption("3", "Remove");
            Console.WriteLine("(B) Back");
            Console.Write("Choice: ");
            string? choice = Console.ReadLine();
            if (IsBack(choice)) return;

            switch (choice)
            {
                case "1":
                    UpdateTask(manager, id);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nTask updated.");
                    Console.ResetColor();
                    Console.ReadKey();
                    break;

                case "2":
                    MarkTasksAsDoneLoop(manager, id);
                    break;

                case "3":
                    manager.RemoveTask(id);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nTask removed.");
                    Console.ResetColor();
                    Console.ReadKey();
                    break;
            }
        }

        // Loop that lets the user mark several tasks as done in a row.
        private static void MarkTasksAsDoneLoop(TaskManager manager, int firstId)
        {
            int currentId = firstId;

            while (true)
            {
                manager.MarkDone(currentId);

                if (manager.Tasks.Any(task => !task.IsDone))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nGood job, keep it going!");
                    Console.ResetColor();

                    while (true)
                    {
                        Console.Write("\nMark another task as done? (Y = yes, B = back): ");
                        string? again = Console.ReadLine();

                        if (IsBack(again)) return;

                        if (!string.IsNullOrWhiteSpace(again) &&
                            again.Trim().Equals("y", StringComparison.OrdinalIgnoreCase))
                        {
                            break; // ask for new ID
                        }

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid choice.");
                        Console.ResetColor();
                    }

                    // show all tasks again so it’s clear which ID to choose
                    Console.Clear();
                    Console.WriteLine("All tasks");
                    var all = manager.GetTasksSortedByDate();
                    PrintTaskTable(all);

                    Console.Write("\nEnter ID to mark as done (B = back): ");
                    string? idInput = Console.ReadLine();
                    if (IsBack(idInput)) return;
                    if (!int.TryParse(idInput, out currentId)) return;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nEverything is done! Time to find something new to do.");
                    Console.ResetColor();
                    Console.ReadKey();
                    return;
                }
            }
        }

        // Update title/project/due date for a task
        private static void UpdateTask(TaskManager manager, int id)
        {
            // These "empty keep" prompts let the user update only the fields they want.
            Console.Write("New title (empty keep, B back): ");
            string? title = Console.ReadLine();
            if (IsBack(title)) return;

            Console.Write("New project (empty keep, B back): ");
            string? project = Console.ReadLine();
            if (IsBack(project)) return;

            Console.Write("New date (yyyy-MM-dd, yyyyMMdd, 'today' or 'tomorrow', empty keep, B back): ");
            string? input = Console.ReadLine();
            if (IsBack(input)) return;

            DateTime? newDate = null;

            // validates date when user chooses to edit (empty = keep old date) :D
            if (!string.IsNullOrWhiteSpace(input)) 
            {
                while (true)
                {
                    if (!TryParseFlexibleDate(input, out DateTime parsed))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Invalid date, try again (formats: yyyy-MM-dd, yyyyMMdd, today, tomorrow) or B to back: ");
                        Console.ResetColor();
                        input = Console.ReadLine();
                        if (IsBack(input)) return;
                        if (string.IsNullOrWhiteSpace(input)) break;
                        continue;
                    }

                    if (parsed.Date < DateTime.Today)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Date in past. Enter future date or B to back: ");
                        Console.ResetColor();
                        input = Console.ReadLine();
                        if (IsBack(input)) return;
                        if (string.IsNullOrWhiteSpace(input)) break;
                        continue;
                    }

                    newDate = parsed;
                    break;
                }
            }

            manager.EditTask(id, title ?? string.Empty, project ?? string.Empty, newDate);
        }

        // re-used table printer so style is consistent everywhere
        private static void PrintTaskTable(List<TodoTask> list)
        {
            Console.WriteLine();
            Console.WriteLine("ID | Status | Date       | Project    | Title");
            Console.WriteLine("-----------------------------------------------");

            if (list == null || list.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No tasks to show.");
                Console.ResetColor();
                return;
            }

            foreach (var task in list)
            {
                var oldColor = Console.ForegroundColor;

                // Colour rows depending on status / due date
                if (task.IsDone)
                    Console.ForegroundColor = ConsoleColor.DarkGray;  // completed
                else if (task.DueDate.Date < DateTime.Today)
                    Console.ForegroundColor = ConsoleColor.Red;       // overdue
                else if (task.DueDate.Date == DateTime.Today)
                    Console.ForegroundColor = ConsoleColor.Yellow;    // due today
                else
                    Console.ForegroundColor = ConsoleColor.Green;     // future

                string status = task.IsDone ? "Done" : "Todo";

                //  limit output so it doesn't break the table
                string project = task.Project ?? "";
                if (project.Length > 10) project = project.Substring(0, 10);

                string title = task.Title ?? "";
                if (title.Length > 30) title = title.Substring(0, 27) + "...";

                Console.WriteLine(
                    $"{task.Id,2} | {status,-5} | {task.DueDate:yyyy-MM-dd} | {project,-10} | {title}");

                Console.ForegroundColor = oldColor;
            }
        }

        // Helper: check if user wants to go back (B/b)
        private static bool IsBack(string? input)
        {
            return !string.IsNullOrWhiteSpace(input) &&
                   input.Trim().Equals("b", StringComparison.OrdinalIgnoreCase);
        }

        // smart date parser for today / tomorrow / yyyyMMdd / normal dates
        private static bool TryParseFlexibleDate(string? input, out DateTime date)
        {
            date = default;
            if (string.IsNullOrWhiteSpace(input))
                return false;

            string trimmed = input.Trim();

            // today / tomorrow
            if (trimmed.Equals("today", StringComparison.OrdinalIgnoreCase))
            {
                date = DateTime.Today;
                return true;
            }

            if (trimmed.Equals("tomorrow", StringComparison.OrdinalIgnoreCase))
            {
                date = DateTime.Today.AddDays(1);
                return true;
            }

            // compact yyyyMMdd like 20251223
            if (trimmed.Length == 8 && trimmed.All(char.IsDigit))
            {
                int year = int.Parse(trimmed.Substring(0, 4));
                int month = int.Parse(trimmed.Substring(4, 2));
                int day = int.Parse(trimmed.Substring(6, 2));

                try
                {
                    date = new DateTime(year, month, day);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            // fallback: normal date parsing (yyyy-MM-dd, locale etc.)
            return DateTime.TryParse(trimmed, out date);
        }
    }
}
