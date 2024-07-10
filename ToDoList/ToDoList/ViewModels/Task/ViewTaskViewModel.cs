using MvvmHelpers.Commands;
using System;
using System.Diagnostics;
using System.Windows.Input;
using ToDoList.Models.Converters;
using ToDoList.Models.Wrappers.Task;
using ToDoList.Services.Task;
using ToDoList.ViewModels.Category;
using ToDoList.Views.Category;
using ToDoList.Views.Task;
using Xamarin.Forms;

namespace ToDoList.ViewModels.Task
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class ViewTaskViewModel : ViewModelBase, IViewTaskViewModel
    {


        private int _id;
        private ReadTaskWrapper _task = null!;
        private readonly ITaskService _taskService;

        public ViewTaskViewModel(ITaskService taskService)
        {
            _taskService = taskService;
            UpdateIsExecutedCommand = new AsyncCommand<ReadTaskWrapper>(OnUpdateIsExecuted);
            EditCommand = new AsyncCommand(OnEdit);

        }

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                LoadTask(value);
            }
        }

        public ReadTaskWrapper Task
        {
            get => _task;
            set
            {
                SetProperty(ref _task, value);
            }
        }

        public ICommand UpdateIsExecutedCommand { get; }
        public ICommand EditCommand { get; }

        public async void LoadTask(int id)
        {
            var taskDto = await _taskService.GetTaskAsync(id);
            Task = taskDto.ToWrapper();
        }

        private async System.Threading.Tasks.Task OnUpdateIsExecuted(ReadTaskWrapper taskWrapper)
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
                OnPropertyChanged(nameof(taskWrapper.IsExecuted));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        private async System.Threading.Tasks.Task OnEdit()
        {
            var route = $"/{nameof(EditTaskPage)}?{nameof(EditTaskViewModel.Id)}={Id}";
            await Xamarin.Forms.Shell.Current.GoToAsync(route);
        }

    }
}

