using ToDoList.Models.Wrappers.Task;
using Xamarin.Forms;

namespace ToDoList.ViewModels.Task
{
    public interface IEditTaskViewModel : IViewModel
    {
        Command CancelCommand { get; }
        int Id { get; set; }
        Command SaveCommand { get; }
        UpdateTaskWrapper Task { get; set; }
    }
}