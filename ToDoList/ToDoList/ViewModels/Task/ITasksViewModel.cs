﻿using System.Collections.ObjectModel;
using ToDoList.Models.Wrappers.Task;
using Xamarin.Forms;

namespace ToDoList.ViewModels.Task
{
    public interface ITasksViewModel : IViewModel
    {
        Command AddTaskCommand { get; }
        int CurrentPage { get; set; }
        Command DeleteTaskCommand { get; }
        GetCategoriesParamsWrapper GetTasksParamsWrapper { get; set; }
        int LastPage { get; set; }
        Command LoadTasksCommand { get; }
        Command NextPageCommand { get; }
        Command PreviousPageCommand { get; }
        ReadTaskWrapper SelectedTask { get; set; }
        ObservableCollection<ReadTaskWrapper> Tasks { get; }
        Command<ReadTaskWrapper> TaskTapped { get; }

        void OnAppearing();
    }
}