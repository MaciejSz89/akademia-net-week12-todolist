using System;

namespace ToDoList.Models.Wrappers.Task
{
    public class UpdateTaskWrapper
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Term { get; set; }
        public bool IsExecuted { get; set; }
        public int CategoryId { get; set; }
    }
}
