using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using ToDoList.Models;
using ToDoList.Views;
using ToDoList.Views.Task;
using Xamarin.Forms;
using ToDoList.Models.Wrappers.Task;
using ToDoList.Services.Task;
using ToDoList.Models.Converters;
using System.Linq;
using MvvmHelpers;
using Android.Webkit;
using System.ComponentModel;
using static Android.Resource;

namespace ToDoList.ViewModels.Task
{
    public class TasksViewModel : ViewModelBase, ITasksViewModel
    {
        private ReadTaskWrapper _selectedCategory;
        private int _currentPage = 1;
        private int _lastPage = 1;
        private GetCategoriesParamsWrapper _getCategoriesParamsWrapper;
        private const int _pageSize = 9;
        private readonly ITaskService _taskService;

        public TasksViewModel(ITaskService taskService)
        {
            Title = "Zadania";
            Tasks = new ObservableRangeCollection<ReadTaskWrapper>();
            LoadTasksCommand = new Command(async () => await OnLoadTasks());
            GetTasksParamsWrapper = new GetCategoriesParamsWrapper
            {
                PageSize = _pageSize,
                PageNumber = 1,
            };

            TaskTapped = new Command<ReadTaskWrapper>(OnTaskSelected);

            AddTaskCommand = new Command(ExecuteAddTaskCommand);
            DeleteTaskCommand = new Command<ReadTaskWrapper>(async (x) => await ExecuteDeleteTaskCommand(x));
            PreviousPageCommand = new Command(async (x) => await ExecutePreviousPageCommand(), ValidatePreviousPage);
            NextPageCommand = new Command(async (x) => await ExecuteNextPageCommand(), ValidateNextPage);
            UpdateIsExecutedCommand = new Command<ReadTaskWrapper>(async (x) => await ExecuteUpdateIsExecutedCommand(x));
            _taskService = taskService;
        }


        public ObservableRangeCollection<ReadTaskWrapper> Tasks { get; }
        public Command LoadTasksCommand { get; }

        public Command AddTaskCommand { get; }
        public Command DeleteTaskCommand { get; }
        public Command PreviousPageCommand { get; }
        public Command NextPageCommand { get; }
        public Command UpdateIsExecutedCommand { get; }
        public Command<ReadTaskWrapper> TaskTapped { get; }
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                SetProperty(ref _currentPage, value);
                GetTasksParamsWrapper.PageNumber = value;
                PreviousPageCommand.ChangeCanExecute();
                NextPageCommand.ChangeCanExecute();
            }
        }
        public int LastPage
        {
            get => _lastPage;
            set
            {
                SetProperty(ref _lastPage, value);
                NextPageCommand.ChangeCanExecute();
            }
        }
        public GetCategoriesParamsWrapper GetTasksParamsWrapper
        {
            get => _getCategoriesParamsWrapper;
            set
            {
                SetProperty(ref _getCategoriesParamsWrapper, value);
            }
        }


        private bool ValidateNextPage(object arg)
        {
            return CurrentPage != LastPage;
        }

        private bool ValidatePreviousPage(object arg)
        {
            return CurrentPage != 1;
        }

        private async System.Threading.Tasks.Task ExecuteNextPageCommand()
        {
            IsBusy = true;
            CurrentPage += 1;
            await LoadTasks();
            IsBusy = false;
        }

        private async System.Threading.Tasks.Task ExecutePreviousPageCommand()
        {
            IsBusy = true;
            CurrentPage -= 1;
            await LoadTasks();
            IsBusy = false;
        }


        private async System.Threading.Tasks.Task ExecuteDeleteTaskCommand(ReadTaskWrapper taskWrapper)
        {
            IsBusy = true;
            if (taskWrapper == null)
                return;

            var dialog = await Shell.Current.DisplayAlert("Usuwanie!", $"Czy na pewno chcesz usunąć zadanie: {taskWrapper.Id}", "Tak", "Nie");

            if (!dialog)
                return;

            await _taskService.DeleteTaskAsync(taskWrapper.Id);

            await LoadTasks();
            IsBusy = false;
        }
        private async System.Threading.Tasks.Task ExecuteUpdateIsExecutedCommand(ReadTaskWrapper taskWrapper)
        {
            if (taskWrapper == null || IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (taskWrapper.IsExecuted)
                {
                    await _taskService.FinishTaskAsync(taskWrapper.Id);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                await LoadTasks(); 
                IsBusy = false;
            }
        }

        private async System.Threading.Tasks.Task OnLoadTasks()
        {
            IsBusy = true;

            try
            {
                await LoadTasks();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async System.Threading.Tasks.Task LoadTasks()
        {
            Tasks.Clear();
            var tasksPage = await _taskService.GetTasksAsync(GetTasksParamsWrapper.ToDto());
            var tasks = tasksPage.Tasks
                                 .OrderBy(x => x.IsExecuted)
                                 .ThenBy(x => x.Id)
                                 .Select(x => x.ToWrapper());

            Tasks.ReplaceRange(tasks);
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedTask = null;
        }

        public ReadTaskWrapper SelectedTask
        {
            get => _selectedCategory;
            set
            {
                SetProperty(ref _selectedCategory, value);
                OnTaskSelected(value);
            }
        }

        private async void ExecuteAddTaskCommand(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(AddTaskPage)}");
        }

        async void OnTaskSelected(ReadTaskWrapper task)
        {
            if (task == null)
                return;

            // This will push the TaskDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ViewTaskPage)}?{nameof(ViewTaskViewModel.Id)}={task.Id}");
        }
    }
}