﻿using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Dtos;
using ToDoList.Models.Converters;

namespace ToDoList.Services.Task
{
    public class TaskService : ITaskService
    {
        private readonly HttpClient _httpClient;
        public TaskService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async System.Threading.Tasks.Task AddTaskAsync(CreateTaskDto taskDto)
        {

            var stringContent = new StringContent(JsonConvert.SerializeObject(taskDto), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("Task", stringContent);
        }

        public async System.Threading.Tasks.Task<bool> DeleteTaskAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Task/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                return true;

            return false;
        }

        public async Task<ReadTaskDto> GetTaskAsync(int id)
        {

            var response = await _httpClient.GetAsync($"Task/{id}");

            var json = await response.Content.ReadAsStringAsync();
            var task = JsonConvert.DeserializeObject<ReadTaskDto>(json);
            if (task == null)
                throw new NullReferenceException(nameof(task)); 
            return task;
        }

        public async Task<ReadTasksPageDto> GetTasksAsync(GetTasksParamsDto param)
        {
            var queryString = param.ToQueryString();
            var response = await _httpClient.GetAsync("Task" + queryString);

            var json = await response.Content.ReadAsStringAsync();
            var taskPage = JsonConvert.DeserializeObject<ReadTasksPageDto>(json);
            if (taskPage == null)
                throw new NullReferenceException(nameof(taskPage));
            return taskPage;
        }

        public async System.Threading.Tasks.Task UpdateTaskAsync(int id, UpdateTaskDto taskDto)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(taskDto), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"Task/{id}", stringContent);
        }

        public async System.Threading.Tasks.Task FinishTaskAsync(int id)
        {

            var stringContent = new StringContent(string.Empty);
            var response = await _httpClient.PutAsync($"Task/{id}/Finish", stringContent);
        }
        public async System.Threading.Tasks.Task RestoreTaskAsync(int id)
        {

            var stringContent = new StringContent(string.Empty);
            var response = await _httpClient.PutAsync($"Task/{id}/Restore", stringContent);
        }
    }
}
