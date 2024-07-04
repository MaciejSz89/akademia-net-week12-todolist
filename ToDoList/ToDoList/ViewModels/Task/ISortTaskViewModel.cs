using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ToDoList.Core;
using ToDoList.Models.Wrappers.Task;

namespace ToDoList.ViewModels.Task
{
    public interface ISortTaskViewModel : IViewModel
    {
        ObservableRangeCollection<string> TaskSortMethods { get; }
        GetTasksParamsWrapper GetTasksParamsWrapper { get; }

    }
}
