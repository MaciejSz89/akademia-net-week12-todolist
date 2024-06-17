using MvvmHelpers.Commands;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ToDoList.Models;
using ToDoList.Models.Converters;
using ToDoList.Models.Wrappers.Task;
using ToDoList.Services.Task;
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
            UpdateIsExecutedCommand = new MvvmHelpers.Commands.Command<ReadTaskWrapper>(async (x) => await OnUpdateIsExecutedCommand(x));
            
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

        public MvvmHelpers.Commands.Command UpdateIsExecutedCommand { get; }

        public async void LoadTask(int id)
        {
            var taskDto = await _taskService.GetTaskAsync(id);
            Task = taskDto.ToWrapper();
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
                OnPropertyChanged(nameof(taskWrapper.IsExecuted));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }


        }
    }
}
