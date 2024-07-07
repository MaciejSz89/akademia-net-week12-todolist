using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
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
        Command LoadFiltersCommand { get; }
    }
}
