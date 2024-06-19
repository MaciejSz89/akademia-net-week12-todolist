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
        private int _currentPage = 0;
        private int _currentPageSize = 0;
        private GetCategoriesParamsWrapper _getTasksParamsWrapper;
        private const int _pageSize = 12;
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
            LoadTasksCommand = new Command(async () => await OnLoadTasksCommand());
            LoadMoreTasksCommand = new Command(async () => await OnLoadMoreTasksCommand());

            _getTasksParamsWrapper = new GetCategoriesParamsWrapper();

            TaskTapped = new Command<ReadTaskWrapper>(OnTaskSelected);

            AddTaskCommand = new Command(OnAddTaskCommand);
            EditTaskCommand = new Command<ReadTaskWrapper>(OnEditTaskCommand);
            DeleteTaskCommand = new Command<ReadTaskWrapper>(async (x) => await OnDeleteTaskCommand(x));
            UpdateIsExecutedCommand = new Command<ReadTaskWrapper>(async (x) => await OnUpdateIsExecutedCommand(x));
            _taskService = taskService;
            _messageDialogService = messageDialogService;
            _navigationService = navigationService;
            _sortMethod = TaskSortMethod.ByTitleAscending;
        }



        public ObservableRangeCollection<ReadTaskWrapper> Tasks { get; }
        public Command LoadTasksCommand { get; }
        public Command LoadMoreTasksCommand { get; }
        public Command EditTaskCommand { get; }

        public Command AddTaskCommand { get; }
        public Command DeleteTaskCommand { get; }

        public Command UpdateIsExecutedCommand { get; }
        public Command<ReadTaskWrapper> TaskTapped { get; }




        public GetCategoriesParamsWrapper GetTasksParamsWrapper
        {
            get => _getTasksParamsWrapper;
            set
            {
                SetProperty(ref _getTasksParamsWrapper, value);
            }
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
                taskWrapper.IsExecuted = !taskWrapper.IsExecuted;
                if (!taskWrapper.IsExecuted)
                {
                    await _taskService.FinishTaskAsync(taskWrapper.Id);
                }
                else
                {
                    await _taskService.RestoreTaskAsync(taskWrapper.Id);
                }
               
                SortTasks();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                taskWrapper.IsExecuted = !taskWrapper.IsExecuted;
            }


        }

        private async System.Threading.Tasks.Task OnLoadTasksCommand()
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
        private async System.Threading.Tasks.Task OnLoadMoreTasksCommand()
        {

            try
            {
                await LoadMoreTasks();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }


        private async System.Threading.Tasks.Task LoadTasks()
        {

            Tasks.Clear();
            GetTasksParamsWrapper.PageSize = _pageSize;
            GetTasksParamsWrapper.PageNumber = 1;
            var tasksPage = await _taskService.GetTasksAsync(GetTasksParamsWrapper.ToDto());
            if (tasksPage.Tasks.Any())
            {

                var tasks = tasksPage.Tasks
                                     .Select(x => x.ToWrapper());

                _currentPage = 1;
                _currentPageSize = tasks.Count();

                Tasks.ReplaceRange(tasks);

                SortTasks();
            }

        }
        private async System.Threading.Tasks.Task LoadMoreTasks()
        {
            if (IsBusy || _currentPage == 0)
                return;

            GetTasksParamsWrapper.PageSize = _pageSize;
            GetTasksParamsWrapper.PageNumber = _pageSize == _currentPageSize ? _currentPage + 1 : _currentPage;

            var tasksPage = await _taskService.GetTasksAsync(GetTasksParamsWrapper.ToDto());


            if (!tasksPage.Tasks.Any()
             || GetTasksParamsWrapper.PageNumber != tasksPage.CurrentPage)
                return;


            var fetchedPageSize = tasksPage.Tasks.Count();
            var tasks = tasksPage.Tasks
                                 .Select(x => x.ToWrapper());

            var newTasksForCurrentPage = tasksPage.CurrentPage == _currentPage
                                      && fetchedPageSize > _currentPageSize;

            var nextPage = tasksPage.CurrentPage == _currentPage + 1;

            if (newTasksForCurrentPage)
            {
                tasks = tasks.Skip(_currentPageSize)
                             .Take(fetchedPageSize - _currentPageSize);

                _currentPageSize += tasks.Count();
                Tasks.AddRange(tasks);
                SortTasks();
                return;
            }

            if (nextPage)
            {
                _currentPageSize = tasks.Count();
                _currentPage += 1;
                Tasks.AddRange(tasks);
                SortTasks();
                return;
            }


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