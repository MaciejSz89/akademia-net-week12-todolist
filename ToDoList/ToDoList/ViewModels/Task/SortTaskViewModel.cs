using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ToDoList.Core;
using ToDoList.Helpers;
using ToDoList.Models.Wrappers.Task;

namespace ToDoList.ViewModels.Task
{
    public class SortTaskViewModel : ViewModelBase, ISortTaskViewModel
    {
        private readonly ITasksViewModel _tasksViewModel;

        public SortTaskViewModel(ITasksViewModel tasksViewModel, 
                                 IEnumDescriptionProvider<TaskSortMethod> taskSortMethodDescriptionProvider)
        {
            _tasksViewModel = tasksViewModel;
            TaskSortMethods = new ObservableRangeCollection<string>();
            TaskSortMethods.AddRange(taskSortMethodDescriptionProvider.GetDescriptions());
        }
        public ObservableRangeCollection<string> TaskSortMethods { get; }

        public GetTasksParamsWrapper GetTasksParamsWrapper => _tasksViewModel.GetTasksParamsWrapper;
           

    }
}
