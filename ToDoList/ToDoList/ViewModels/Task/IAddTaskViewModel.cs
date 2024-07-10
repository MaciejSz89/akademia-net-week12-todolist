using System;
using System.Windows.Input;
using ToDoList.Models.Wrappers.Task;

namespace ToDoList.ViewModels.Task
{
    public interface IAddTaskViewModel: IViewModel
    {
        ICommand CancelCommand { get; }
        ICommand SaveCommand { get; }
        CreateTaskWrapper Task { get; set; }
        DateTime Term { get; set; }
    }
}