using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Dtos;
using ToDoList.Exceptions;
using ToDoList.Models.Converters;
using ToDoList.Models.Wrappers.Task;
using Xamarin.Forms;

namespace ToDoList.Services.Task
{
    public class TaskService : ITaskService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public TaskService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async System.Threading.Tasks.Task AddTaskAsync(CreateTaskDto taskDto)
        {
            using (var httpClient = _httpClientFactory.CreateClient("httpClient"))
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(taskDto), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("Task", stringContent);

                if (response.StatusCode == HttpStatusCode.Forbidden
                 || response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException();
                else if (!(response.StatusCode == HttpStatusCode.Created))
                    throw new EntityOperationException(EntityType.Task, OperationType.Create);
            }
        }

        public async System.Threading.Tasks.Task DeleteTaskAsync(int id)
        {
            using (var httpClient = _httpClientFactory.CreateClient("httpClient"))
            {
                var response = await httpClient.DeleteAsync($"Task/{id}");

                if (response.StatusCode == HttpStatusCode.Forbidden
                 || response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException();
                else if (!(response.StatusCode == HttpStatusCode.NoContent))
                    throw new EntityOperationException(EntityType.Task, OperationType.Delete);
            }
        }

        public async Task<ReadTaskDto> GetTaskAsync(int id)
        {
            using (var httpClient = _httpClientFactory.CreateClient("httpClient"))
            {
                var response = await httpClient.GetAsync($"Task/{id}");

                if (response.StatusCode == HttpStatusCode.Forbidden
                 || response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException();
                else if (!(response.StatusCode == HttpStatusCode.OK))
                    throw new EntityOperationException(EntityType.Task, OperationType.Read);

                var json = await response.Content.ReadAsStringAsync();
                var task = JsonConvert.DeserializeObject<ReadTaskDto>(json);
                return task;
            }
        }

        public async Task<ReadTasksPageDto> GetTasksAsync(GetTasksParamsDto param)
        {
            using (var httpClient = _httpClientFactory.CreateClient("httpClient"))
            {
                var queryString = param.ToQueryString();
                var response = await httpClient.GetAsync("Task" + queryString);

                if (response.StatusCode == HttpStatusCode.Forbidden
                 || response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException();
                else if (!(response.StatusCode == HttpStatusCode.OK))
                    throw new EntityOperationException(EntityType.Task, OperationType.ReadMany);

                var json = await response.Content.ReadAsStringAsync();
                var taskPage = JsonConvert.DeserializeObject<ReadTasksPageDto>(json);
                return taskPage;
            }
        }

        public async System.Threading.Tasks.Task UpdateTaskAsync(int id, UpdateTaskDto taskDto)
        {
            using (var httpClient = _httpClientFactory.CreateClient("httpClient"))
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(taskDto), Encoding.UTF8, "application/json");

                var response = await httpClient.PutAsync($"Task/{id}", stringContent);

                if (response.StatusCode == HttpStatusCode.Forbidden
                 || response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException();
                else if (!(response.StatusCode == HttpStatusCode.OK))
                    throw new EntityOperationException(EntityType.Task, OperationType.Update);
            }
        }

        public async System.Threading.Tasks.Task FinishTaskAsync(int id)
        {
            using (var httpClient = _httpClientFactory.CreateClient("httpClient"))
            {
                var stringContent = new StringContent(string.Empty);
                var response = await httpClient.PutAsync($"Task/{id}/Finish", stringContent);

                if (response.StatusCode == HttpStatusCode.Forbidden
                 || response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException();
                else if (!(response.StatusCode == HttpStatusCode.OK))
                    throw new EntityOperationException(EntityType.Task, OperationType.Update);
            }
        }
    }
}
