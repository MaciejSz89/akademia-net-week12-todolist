using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models.Converters;
using ToDoList.Models.Wrappers.Task;
using ToDoList.Services.Task;
using Xamarin.Forms;

namespace ToDoList.ViewModels.Task
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class EditTaskViewModel : BaseViewModel, IEditTaskViewModel
    {
        private UpdateTaskWrapper _task;
        private int _id;
        private readonly ITaskService _taskService;

        public EditTaskViewModel(ITaskService taskService, int taskId)
        {         
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
            _taskService = taskService;
            Id = taskId;
            LoadTask(taskId);
        }

        private bool ValidateSave()
        {
            //TODO: Add validation
            return true;
        }

        public UpdateTaskWrapper Task
        {
            get => _task;
            set
            {
                SetProperty(ref _task, value);
            }
        }

        public int Id
        {
            get => _id;
            set
            {
                SetProperty(ref _id, value);
                LoadTask(value);
            }
        }

        private async void LoadTask(int id)
        {
            Task = (await _taskService.GetTaskAsync(id)).ToUpdateWrapper();
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
            await _taskService.UpdateTaskAsync(Id, Task.ToDto());

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("../../");
        }
    }
}
