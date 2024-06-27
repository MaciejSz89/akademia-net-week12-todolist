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
        public ObservableCollection<string> TaskSortMethodDescriptions { get; }
        private readonly IEnumDescriptionProvider<TaskSortMethod> _taskSortMethodDescriptionProvider;
        private TaskSortMethod? _selectedTaskSortMethod;

        public SortTaskViewModel(TaskSortMethod? selectedMethod)
        {
            _taskSortMethodDescriptionProvider = new TaskSortMethodDescriptionProvider();
        }

        public string SelectedTaskSortMethodDescription
        {
            get => _selectedTaskSortMethodDescription;
            set
            {
                SetProperty(ref _selectedTaskSortMethodDescription, value);
                SelectedTaskSortMethod = EnumHelper.GetEnumFromDescription<TaskSortMethod>(_selectedTaskSortMethodDescription, _taskSortMethodDescriptionProvider);
            }
        }

        public TaskSortMethod? SelectedTaskSortMethod
        {
            get => _selectedTaskSortMethod;
            set
            {
                _selectedTaskSortMethod = value;
                OnPropertyChanged();
            }
        }

    }
}
