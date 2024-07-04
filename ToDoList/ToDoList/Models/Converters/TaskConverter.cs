using ToDoList.Core;
using ToDoList.Core.Dtos;
using ToDoList.Helpers;
using ToDoList.Models.Wrappers.Task;
using ToDoList.ViewModels.Task;

namespace ToDoList.Models.Converters
{
    public static class TaskConverter
    {
        public static GetTasksParamsDto ToDto(this GetTasksParamsWrapper wrapper)
        {
            var taskSortMethodDescriptionProvider = new TaskSortMethodDescriptionProvider();
            var sortMethod = EnumHelper.GetEnumFromDescription<TaskSortMethod>(wrapper.SortMethod, taskSortMethodDescriptionProvider) ?? TaskSortMethod.ByTitleAscending;
            var getTasksParamsDto = new GetTasksParamsDto
            {
                Title = wrapper.Title,
                CategoryId = wrapper.CategoryId,
                IsExecuted = wrapper.IsExecuted,
                PageNumber = wrapper.PageNumber,
                PageSize = wrapper.PageSize,
                SortMethod = sortMethod
            };
            return getTasksParamsDto;
        }


        public static ReadTaskWrapper ToWrapper(this ReadTaskDto dto)
        {
            var readTaskWrapper = new ReadTaskWrapper
            {
                Id = dto.Id,
                Title = dto.Title,
                CategoryId = dto.CategoryId,
                CategoryName = dto.CategoryName,
                IsExecuted = dto.IsExecuted,
                Description = dto.Description,
                Term = dto.Term
            };
            return readTaskWrapper;
        }

        public static CreateTaskDto ToDto(this CreateTaskWrapper wrapper)
        {
            var createTaskDto = new CreateTaskDto
            {
               CategoryId = wrapper.CategoryId,
               Title = wrapper.Title,
               Description = wrapper.Description,
               Term = wrapper.Term
            };
            return createTaskDto;
        }

        public static UpdateTaskDto ToDto(this UpdateTaskWrapper wrapper)
        {
            var updateTaskDto = new UpdateTaskDto
            {
                CategoryId = wrapper.CategoryId,
                Title = wrapper.Title,
                Description = wrapper.Description,
                Term = wrapper.Term,
                IsExecuted = wrapper.IsExecuted
            };
            return updateTaskDto;
        }

        public static UpdateTaskWrapper ToUpdateWrapper(this ReadTaskDto dto)
        {
            var updateTaskWrapper = new UpdateTaskWrapper
            {
                Title = dto.Title,
                CategoryId = dto.CategoryId,
                IsExecuted = dto.IsExecuted,
                Description = dto.Description,
                Term = dto.Term
            };
            return updateTaskWrapper;
        }
    }
}
