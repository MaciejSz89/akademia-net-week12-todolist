using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using ToDoList.Models;
using ToDoList.Models.Converters;
using ToDoList.Models.Wrappers.Category;
using ToDoList.Models.Wrappers.Task;
using ToDoList.Services.Category;
using ToDoList.Services.Task;
using Xamarin.Forms;

namespace ToDoList.ViewModels.Task
{
    public class AddTaskViewModel : ViewModelBase, IAddTaskViewModel
    {
        private CreateTaskWrapper _task;
        private readonly ITaskService _taskService;
        private readonly ICategoryService _categoryService;

        public AddTaskViewModel(ITaskService taskService, ICategoryService categoryService)
        {
            Task = new CreateTaskWrapper();
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
            _taskService = taskService;
            _categoryService = categoryService;
            var categoryDtos = _categoryService.GetCategoriesAsync().Result;
            Categories = new ObservableCollection<ReadCategoryWrapper>();
            foreach (var dto in categoryDtos)
            {
                Categories.Add(dto.ToWrapper());
            }
        }

        private bool ValidateSave()
        {
            //TODO: Add validation
            return true;
        }

        public ObservableCollection<ReadCategoryWrapper> Categories { get; set; }

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
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {


            await _taskService.AddTaskAsync(Task.ToDto());

            await Shell.Current.GoToAsync("..");
        }
    }
}
