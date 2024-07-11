using MvvmHelpers;
using System.Windows.Input;
using ToDoList.Models.Wrappers.Task;

namespace ToDoList.ViewModels.Task
{
    public interface ITasksViewModel : IViewModel
    {
        GetTasksParamsWrapper GetTasksParamsWrapper { get; set; }
        ICommand AddTaskCommand { get; }
        ICommand LoadTasksCommand { get; }
        ICommand DeleteTaskCommand { get; }
        ICommand EditTaskCommand { get; }
        ICommand LoadMoreTasksCommand { get; }
        ReadTaskWrapper? SelectedTask { get; set; }
        ObservableRangeCollection<ReadTaskWrapper> Tasks { get; }
        ICommand TaskTapped { get; }
        ICommand UpdateIsExecutedCommand { get; }
        ICommand SelectSortMethodCommand { get; }
        ICommand SelectFiltersCommand { get; }        
        void OnAppearing();
    }
}