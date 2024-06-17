using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private CreateTaskWrapper _task = null!;
        private ReadCategoryWrapper _selectedCategory = null!;
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
            Categories = new ObservableCollection<ReadCategoryWrapper>();
            Initialize();
        }

        private async void Initialize()
        {
            Term = DateTime.Now;
            var categoryDtos = await _categoryService.GetCategoriesAsync();
            if (categoryDtos != null && categoryDtos.Any())
            {
                foreach (var dto in categoryDtos)
                {
                    if (dto == null)
                        continue;
                    Categories.Add(dto.ToWrapper());
                }
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

        public DateTime Term
        {
            get => Task.Term is null ? DateTime.Now : (DateTime)Task.Term;
            set
            {
                Task.Term = value;
                OnPropertyChanged(nameof(Task.Term));
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
        public ReadCategoryWrapper SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                SetProperty(ref _selectedCategory, value);
                Task.CategoryId = _selectedCategory.Id;
            }
        }

    }
}
