using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ToDoList
{
    // TaskManager is the BOZZZ (boss) of all tasks
    // Just check what it knows:
    // - which tasks exist
    // - how to add / edit / remove them
    // - how to sort and filter them
    // - how to save/load them from a file (so they don’t vanish when you close the app)
    public class TaskManager
    {
        // Here we store all tasks in a simple List.
        // No LinkedList-fancy stuff
        private readonly List<TodoTask> _tasks = new List<TodoTask>();

        // This is our little ID-generator.
        // Every new task gets the next number.
        private int _nextId = 1;

        // Expose tasks read-only-ish: outside code can look, foreach and LINQ,
        // but can't do _tasks.Add(...) directly.
        public IEnumerable<TodoTask> Tasks => _tasks;

        // Adds new task to list :D
        public void AddTask(string title, string project, DateTime dueDate)
        {
            var task = new TodoTask
            {
                Id = _nextId++,
                Title = title,
                Project = project,
                DueDate = dueDate,
                IsDone = false
            };

            _tasks.Add(task);
        }

        // Edit an existing task if we can find it
        public void EditTask(int id, string newTitle, string newProject, DateTime? newDueDate)
        {
            // LINQ because we’re fancy enough for that at least :)
            var task = _tasks.FirstOrDefault(tasks => tasks.Id == id);
            if (task == null)
                return;

            if (!string.IsNullOrWhiteSpace(newTitle))
                task.Title = newTitle;

            if (!string.IsNullOrWhiteSpace(newProject))
                task.Project = newProject;

            if (newDueDate.HasValue)
                task.DueDate = newDueDate.Value;
        }

        // Marks a task as done
        public void MarkDone(int id)
        {
            var task = _tasks.FirstOrDefault(tasks => tasks.Id == id);
            if (task != null)
                task.IsDone = true;
        }

        public void MarkTodo(int id)
        {
            var task = Tasks.FirstOrDefault(t  => t.Id == id);
            if (task == null) return;

            task.IsDone = false;
        }

        // Remove task completely from list
        public void RemoveTask(int id)
        {
            var task = _tasks.FirstOrDefault(tasks => tasks.Id == id);
            if (task != null)
                _tasks.Remove(task);
        }

        // Sorting & filtering (still using LINQ) 

        public List<TodoTask> GetTasksSortedByDate()
        {
            return _tasks
                .OrderBy(tasks => tasks.DueDate)
                .ToList();
        }

        // Returns a new list sorted by project, then by date inside each project.
        public List<TodoTask> GetTasksSortedByProject()
        {
            return _tasks
                .OrderBy(tasks => tasks.Project)
                .ThenBy(tasks => tasks.DueDate)
                .ToList();
        }

        public List<TodoTask> FilterByProject(string project)
        {
            return _tasks
                .Where(tasks => tasks.Project.Equals(project, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // Save / Load (persistent state) 
        // File format: id;title;dueDate;isDone;project

        public void LoadFromFile(string path)
        {
            if (!File.Exists(path))
                return;

            _tasks.Clear();

            // Read everything line by line from the file.
            foreach (string line in File.ReadAllLines(path))
            {
                var parts = line.Split(';');
                if (parts.Length != 5)
                    continue;

                var task = new TodoTask
                {
                    Id = int.Parse(parts[0]),
                    Title = parts[1],
                    DueDate = DateTime.Parse(parts[2]),
                    IsDone = bool.Parse(parts[3]),
                    Project = parts[4]
                };

                _tasks.Add(task);
            }

            // Make sure _nextId continues after the highest existing id
            if (_tasks.Any())
                _nextId = _tasks.Max(tasks => tasks.Id) + 1;
            else
                _nextId = 1;
        }

        public void SaveToFile(string path)
        {
            var lines = _tasks.Select(tasks =>
                $"{tasks.Id};{tasks.Title};{tasks.DueDate:yyyy-MM-dd};{tasks.IsDone};{tasks.Project}");

            File.WriteAllLines(path, lines.ToList());
        }
    }
}
