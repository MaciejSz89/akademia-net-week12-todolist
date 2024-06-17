using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using ToDoList.Models;
using ToDoList.Views;
using ToDoList.Views.Task;
using ToDoList.Models.Wrappers.Task;
using ToDoList.Services.Task;
using ToDoList.Models.Converters;
using System.Linq;
using MvvmHelpers;
using Android.Webkit;
using System.ComponentModel;
using static Android.Resource;
using System.Collections.Generic;
using Xamarin.Forms.Internals;
using MvvmHelpers.Commands;
using ToDoList.Services.MessageDialog;
using ToDoList.Services.Navigation;


public enum TaskSortMethod
{
    ByIdAscending,
    ByIdDescending,
    ByTitleAscending,
    ByTitleDescending,
    ByTermAscending,
    ByTermDescending

}

namespace ToDoList.ViewModels.Task
{
    public class TasksViewModel : ViewModelBase, ITasksViewModel
    {
        private ReadTaskWrapper? _selectedTask;
        private int _currentPage = 1;
        private int _lastPage = 1;
        private GetCategoriesParamsWrapper _getTasksParamsWrapper;
        private const int _pageSize = 9;
        private readonly ITaskService _taskService;
        private readonly IMessageDialogService _messageDialogService;
        private readonly INavigationService _navigationService;
        private TaskSortMethod _sortMethod;

        public TasksViewModel(ITaskService taskService,
                              IMessageDialogService messageDialogService,
                              INavigationService navigationService)
        {
            Title = "Zadania";
            Tasks = new ObservableRangeCollection<ReadTaskWrapper>();
            LoadTasksCommand = new Command(async () => await ExecuteLoadTasksCommand());

            _getTasksParamsWrapper = new GetCategoriesParamsWrapper
            {
                PageSize = _pageSize,
                PageNumber = 1,
            };

            TaskTapped = new Command<ReadTaskWrapper>(OnTaskSelected);

            AddTaskCommand = new Command(OnAddTaskCommand);
            EditTaskCommand = new Command<ReadTaskWrapper>(OnEditTaskCommand);
            DeleteTaskCommand = new Command<ReadTaskWrapper>(async (x) => await OnDeleteTaskCommand(x));
            PreviousPageCommand = new Command(async (x) => await OnPreviousPageCommand(), ValidatePreviousPage);
            NextPageCommand = new Command(async (x) => await OnNextPageCommand(), ValidateNextPage);
            UpdateIsExecutedCommand = new Command<ReadTaskWrapper>(async (x) => await OnUpdateIsExecutedCommand(x));
            _taskService = taskService;
            _messageDialogService = messageDialogService;
            _navigationService = navigationService;
            _sortMethod = TaskSortMethod.ByTitleAscending;
        }


        public ObservableRangeCollection<ReadTaskWrapper> Tasks { get; }
        public Command LoadTasksCommand { get; }
        public Command EditTaskCommand { get; }

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
                PreviousPageCommand.RaiseCanExecuteChanged();
                NextPageCommand.RaiseCanExecuteChanged();
            }
        }
        public int LastPage
        {
            get => _lastPage;
            set
            {
                SetProperty(ref _lastPage, value);
                NextPageCommand.RaiseCanExecuteChanged();
            }
        }
        public GetCategoriesParamsWrapper GetTasksParamsWrapper
        {
            get => _getTasksParamsWrapper;
            set
            {
                SetProperty(ref _getTasksParamsWrapper, value);
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

        private async System.Threading.Tasks.Task OnNextPageCommand()
        {

            IsBusy = true;
            CurrentPage += 1;
            await LoadTasks();
            IsBusy = false;
        }

        private async System.Threading.Tasks.Task OnPreviousPageCommand()
        {

            IsBusy = true;
            CurrentPage -= 1;
            await LoadTasks();
            IsBusy = false;
        }



        private async System.Threading.Tasks.Task OnDeleteTaskCommand(ReadTaskWrapper taskWrapper)
        {


            if (taskWrapper == null)
                return;

            var dialog = await _messageDialogService.ShowMessageConfirmAsync("Usuwanie!", $"Czy na pewno chcesz usunąć zadanie: {taskWrapper.Title}");

            if (!dialog)
                return;

            if (await _taskService.DeleteTaskAsync(taskWrapper.Id))
                Tasks.Remove(taskWrapper);



        }
        private async System.Threading.Tasks.Task OnUpdateIsExecutedCommand(ReadTaskWrapper taskWrapper)
        {

            if (taskWrapper == null || IsBusy)
                return;



            try
            {
                if (!taskWrapper.IsExecuted)
                {
                    await _taskService.FinishTaskAsync(taskWrapper.Id);
                }
                else
                {
                    await _taskService.RestoreTaskAsync(taskWrapper.Id);

                }
                taskWrapper.IsExecuted = !taskWrapper.IsExecuted;
                SortTasks();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }


        }

        private async System.Threading.Tasks.Task ExecuteLoadTasksCommand()
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
                                 .Select(x => x.ToWrapper());
            Tasks.ReplaceRange(tasks);
            SortTasks();
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedTask = null;
        }

        public ReadTaskWrapper? SelectedTask
        {
            get => _selectedTask;
            set
            {
                SetProperty(ref _selectedTask, value);
                OnTaskSelected(value);
            }
        }


        private async void OnAddTaskCommand(object obj)
        {
           await _navigationService.GoToPageRelative(nameof(AddTaskPage));
        }

        private async void OnEditTaskCommand(ReadTaskWrapper taskWrapper)
        {
            await _navigationService.GoToPageRelative(nameof(EditTaskPage), taskWrapper.Id.ToString());
        }
        async void OnTaskSelected(ReadTaskWrapper? task)
        {
            if (task == null)
                return;

            // This will push the TaskDetailPage onto the navigation stack
            var param = new KeyValuePair<string, string>(nameof(ViewTaskViewModel.Id), task.Id.ToString());
            await _navigationService.GoToPageRelative(nameof(ViewTaskPage), param);
        }

        private void SortTasks()
        {

            var tasks = new List<ReadTaskWrapper>(Tasks);


            switch (_sortMethod)
            {
                case TaskSortMethod.ByIdAscending:
                    tasks = Tasks.OrderBy(x => x.IsExecuted)
                                 .ThenBy(x => x.Id)
                                 .ToList();
                    break;
                case TaskSortMethod.ByIdDescending:
                    tasks = Tasks.OrderBy(x => x.IsExecuted)
                                 .ThenByDescending(x => x.Id)
                                 .ToList();
                    break;
                case TaskSortMethod.ByTitleAscending:
                    tasks = Tasks.OrderBy(x => x.IsExecuted)
                                 .ThenBy(x => x.Title)
                                 .ToList();
                    break;
                case TaskSortMethod.ByTitleDescending:
                    tasks = Tasks.OrderBy(x => x.IsExecuted)
                                 .ThenByDescending(x => x.Title)
                                 .ToList();
                    break;
                case TaskSortMethod.ByTermAscending:
                    tasks = Tasks.OrderBy(x => x.IsExecuted)
                                 .ThenBy(x => x.Term)
                                 .ToList();
                    break;
                case TaskSortMethod.ByTermDescending:
                    tasks = Tasks.OrderBy(x => x.IsExecuted)
                                 .ThenByDescending(x => x.Term)
                                 .ToList();
                    break;
                default:
                    break;
            }
            Tasks.ReplaceRange(tasks);


        }
    }
}