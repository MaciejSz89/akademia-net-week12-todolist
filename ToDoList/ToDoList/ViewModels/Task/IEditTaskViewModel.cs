using MvvmHelpers;
using System.Windows.Input;
using ToDoList.Models.Wrappers.Category;
using ToDoList.Models.Wrappers.Task;

namespace ToDoList.ViewModels.Task
{
    public interface IEditTaskViewModel : IViewModel
    {
        int Id { get; set; }
        ICommand SaveCommand { get; }
        ICommand CancelCommand { get; }
        UpdateTaskWrapper Task { get; set; }
        ReadCategoryWrapper SelectedCategory { get; set; }
        ObservableRangeCollection<ReadCategoryWrapper> Categories { get; }
    }
}