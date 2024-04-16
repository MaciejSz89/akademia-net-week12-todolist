using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ToDoList.Models;
using ToDoList.Models.Converters;
using ToDoList.Models.Wrappers.Task;
using ToDoList.Services.Task;
using Xamarin.Forms;

namespace ToDoList.ViewModels.Task
{
    public class AddTaskViewModel : BaseViewModel, IAddTaskViewModel
    {
        private CreateTaskWrapper _task;
        private readonly ITaskService _taskService;

        public AddTaskViewModel(ITaskService taskService)
        {
            Task = new CreateTaskWrapper();
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
            _taskService = taskService;
        }

        private bool ValidateSave()
        {
            //TODO: Add validation
            return true;
        }

        public CreateTaskWrapper Task
        {
            get => _task;
            set
            {
                SetProperty(ref _task, value);
            }
        }



        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {


            await _taskService.AddTaskAsync(Task.ToDto());

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
