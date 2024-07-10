using Android.Webkit;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Linq;
using System.Windows.Input;
using ToDoList.Models.Converters;
using ToDoList.Models.Wrappers.Category;
using ToDoList.Models.Wrappers.Task;
using ToDoList.Services.Category;
using ToDoList.Services.Task;
using Xamarin.Forms;

namespace ToDoList.ViewModels.Task
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class EditTaskViewModel : ViewModelBase, IEditTaskViewModel
    {
        private UpdateTaskWrapper _task = null!;
        private int _id;
        private ReadCategoryWrapper _selectedCategory;
        private readonly ITaskService _taskService;
        private readonly ICategoryService _categoryService;

        public EditTaskViewModel(ITaskService taskService,
                                 ICategoryService categoryService)
        {
            SaveCommand = new AsyncCommand(OnSave, ValidateSave);
            CancelCommand = new AsyncCommand(OnCancel);
            PropertyChanged +=
                (_, __) => (SaveCommand as AsyncCommand)?.RaiseCanExecuteChanged();
            _taskService = taskService;
            _categoryService = categoryService;
            Categories = new ObservableRangeCollection<ReadCategoryWrapper>();
            Device.BeginInvokeOnMainThread(async () => await GetCategoriesAsync());
        }

        private bool ValidateSave(object obj)
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
                System.Threading.Tasks.Task.Run(() => LoadTask(value));
            }
        }

        private async System.Threading.Tasks.Task LoadTask(int id)
        {
            Task = (await _taskService.GetTaskAsync(id)).ToUpdateWrapper();
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public ObservableRangeCollection<ReadCategoryWrapper> Categories { get; }
        public ReadCategoryWrapper SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                SetProperty(ref _selectedCategory, value);
                Task.CategoryId = value.Id;
            }
        }

        private async System.Threading.Tasks.Task OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async System.Threading.Tasks.Task OnSave()
        {
            await _taskService.UpdateTaskAsync(Id, Task.ToDto());

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("../../");
        }

        private async System.Threading.Tasks.Task GetCategoriesAsync()
        {
            var categoryDtos = await _categoryService.GetCategoriesAsync();

            Categories.AddRange(categoryDtos.Select(c => c.ToWrapper()));
            SelectedCategory = (await _categoryService.GetCategoryAsync(Task.CategoryId)).ToWrapper();
        }
    }
}
