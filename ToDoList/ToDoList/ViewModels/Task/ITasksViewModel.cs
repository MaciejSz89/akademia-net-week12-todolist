using MvvmHelpers;
using MvvmHelpers.Commands;
using ToDoList.Models.Wrappers.Task;

namespace ToDoList.ViewModels.Task
{
    public interface ITasksViewModel : IViewModel
    {
        Command AddTaskCommand { get; }
        Command DeleteTaskCommand { get; }
        GetCategoriesParamsWrapper GetTasksParamsWrapper { get; set; }
        Command EditTaskCommand { get; }
        Command LoadTasksCommand { get; }
        Command LoadMoreTasksCommand { get; }
        ReadTaskWrapper? SelectedTask { get; set; }
        ObservableRangeCollection<ReadTaskWrapper> Tasks { get; }
        Command<ReadTaskWrapper> TaskTapped { get; }
        Command UpdateIsExecutedCommand { get; }

        void OnAppearing();
    }
}