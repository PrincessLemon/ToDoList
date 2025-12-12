using System;

namespace ToDoList
{
    // Represents one task in the todo list
    public class TodoTask
    {
        public int Id { get; set; }               // Unique numeric id (1, 2, 3...)
        public string Title { get; set; } = string.Empty; // Task name / description / NO GREEN STUFF
        public DateTime DueDate { get; set; }     // When the task is due
        public string Project { get; set; } = string.Empty;     // Project or category name
        public bool IsDone { get; set; }          // true = completed, false = todo

        // Defines how the task will be printed in the console
        public override string ToString()
        {
            string status = IsDone ? "Done " : "Todo ";
            return $"{Id,2} | {status} | {DueDate:yyyy-MM-dd} | {Project,-10} | {Title}";
        }
    }
}
