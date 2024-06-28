using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ToDoList.Core;

namespace ToDoList.ViewModels.Task
{
    public interface ISortTaskViewModel : IViewModel
    {
        TaskSortMethod? SelectedTaskSortMethod { get; set; }
        string SelectedTaskSortMethodDescription { get; set; }

    }
}
