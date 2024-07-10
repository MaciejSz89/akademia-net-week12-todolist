using MvvmHelpers;
using System.Collections.Generic;
using System.Windows.Input;
using ToDoList.Models.Wrappers.Category;
using ToDoList.Models.Wrappers.Task;

namespace ToDoList.ViewModels.Task
{
    public interface IFilterTaskViewModel : IViewModel
    {
        ObservableRangeCollection<ReadCategoryWrapper?> Categories { get; }
        GetTasksParamsWrapper GetTasksParamsWrapper { get; }
        ReadCategoryWrapper? SelectedCategoryFilter { get; set; }
        ICommand LoadFiltersCommand { get; }
        ICommand ClearFiltersCommand { get; }
        ObservableRangeCollection<KeyValuePair<string, bool?>> IsExecutedFilters { get; }
        KeyValuePair<string, bool?> SelectedIsExecutedFilter { get; set; }
    }
}
