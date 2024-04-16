using ToDoList.Models.Wrappers.Task;
using Xamarin.Forms;

namespace ToDoList.ViewModels.Task
{
    public interface IAddTaskViewModel: IViewModel
    {
        Command CancelCommand { get; }
        Command SaveCommand { get; }
        CreateTaskWrapper Task { get; set; }
    }
}