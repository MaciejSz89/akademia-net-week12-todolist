using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ToDoList.Core;
using ToDoList.Helpers;

namespace ToDoList.ViewModels.Task
{
    public class SortTaskViewModel : ViewModelBase, ISortTaskViewModel
    {
        private string _selectedTaskSortMethodDescription;
        private readonly IEnumDescriptionProvider<TaskSortMethod> _taskSortMethodDescriptionProvider;
        private readonly ITasksViewModel _tasksViewModel;
        private TaskSortMethod? _selectedTaskSortMethod;

        public SortTaskViewModel(ITasksViewModel tasksViewModel)
        {
            _tasksViewModel = tasksViewModel;
            _taskSortMethodDescriptionProvider = new TaskSortMethodDescriptionProvider();
            _selectedTaskSortMethodDescription = _taskSortMethodDescriptionProvider.GetDescription(_tasksViewModel.GetTasksParamsWrapper.SortMethod);
            TaskSortMethodDescriptions = new ObservableRangeCollection<string>();
            TaskSortMethodDescriptions.AddRange(_taskSortMethodDescriptionProvider.GetDescriptions());
        }
        public ObservableRangeCollection<string> TaskSortMethodDescriptions { get; }

        public string SelectedTaskSortMethodDescription
        {
            get => _selectedTaskSortMethodDescription;
            set
            {
                SetProperty(ref _selectedTaskSortMethodDescription, value);
                var previouslySelectedTaskSortMethod = _selectedTaskSortMethod;
                SelectedTaskSortMethod = EnumHelper.GetEnumFromDescription<TaskSortMethod>(_selectedTaskSortMethodDescription, _taskSortMethodDescriptionProvider);
                
                if (_selectedTaskSortMethod != null && previouslySelectedTaskSortMethod != _selectedTaskSortMethod)
                {
                    _tasksViewModel.GetTasksParamsWrapper.SortMethod = (TaskSortMethod)_selectedTaskSortMethod;
                    _tasksViewModel.IsBusy = true;
                }
              
            }
        }

        public TaskSortMethod? SelectedTaskSortMethod
        {
            get => _selectedTaskSortMethod;
            set
            {
                SetProperty(ref _selectedTaskSortMethod, value);
            }
        }

    }
}
