using MvvmHelpers;
using ToDoList.Models.Wrappers.Task;

namespace ToDoList.ViewModels.Task
{
    public interface ISortTaskViewModel : IViewModel
    {
        ObservableRangeCollection<string> TaskSortMethods { get; }
        GetTasksParamsWrapper GetTasksParamsWrapper { get; }

    }
}
