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
        private ReadTaskWrapper _task;
        private readonly ITaskService _taskService;

        public ViewTaskViewModel(ITaskService taskService)
        {
            _taskService = taskService;
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

        public async void LoadTask(int id)
        {
            var taskDto = await _taskService.GetTaskAsync(id);
            Task = taskDto.ToWrapper();
        }
    }
}
