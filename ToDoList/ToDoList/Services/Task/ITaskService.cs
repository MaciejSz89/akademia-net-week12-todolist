using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Dtos;

namespace ToDoList.Services.Task
{
    public interface ITaskService
    {
        System.Threading.Tasks.Task AddTaskAsync(CreateTaskDto taskDto);
        System.Threading.Tasks.Task UpdateTaskAsync(int id, UpdateTaskDto taskDto);
        System.Threading.Tasks.Task<bool> DeleteTaskAsync(int id);
        Task<ReadTaskDto> GetTaskAsync(int id);
        Task<ReadTasksPageDto> GetTasksAsync(GetTasksParamsDto param);
        System.Threading.Tasks.Task FinishTaskAsync(int id);
        System.Threading.Tasks.Task RestoreTaskAsync(int id);
    }
}
