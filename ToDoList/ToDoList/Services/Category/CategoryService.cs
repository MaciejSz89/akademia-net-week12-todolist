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
using ToDoList.Models.Wrappers.Category;
using Xamarin.Forms;

namespace ToDoList.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CategoryService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async System.Threading.Tasks.Task AddCategoryAsync(WriteCategoryDto categoryDto)
        {
            using (var httpClient = _httpClientFactory.CreateClient("httpClient"))
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(categoryDto), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("Category", stringContent);

                if (response.StatusCode == HttpStatusCode.Forbidden
                 || response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException();
                else if (!(response.StatusCode == HttpStatusCode.Created))
                    throw new EntityOperationException(EntityType.Category, OperationType.Create);
            }
        }

        public async System.Threading.Tasks.Task DeleteCategoryAsync(int id)
        {
            using (var httpClient = _httpClientFactory.CreateClient("httpClient"))
            {
                var response = await httpClient.DeleteAsync($"Category/{id}");

                if (response.StatusCode == HttpStatusCode.Forbidden
                 || response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException();
                else if (!(response.StatusCode == HttpStatusCode.NoContent))
                    throw new EntityOperationException(EntityType.Category, OperationType.Delete);
            }
        }

        public async Task<ReadCategoryDto> GetCategoryAsync(int id)
        {
            using (var httpClient = _httpClientFactory.CreateClient("httpClient"))
            {
                var response = await httpClient.GetAsync($"Category/{id}");

                if (response.StatusCode == HttpStatusCode.Forbidden
                 || response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException();
                else if (!(response.StatusCode == HttpStatusCode.OK))
                    throw new EntityOperationException(EntityType.Category, OperationType.Read);

                var json = await response.Content.ReadAsStringAsync();
                var category = JsonConvert.DeserializeObject<ReadCategoryDto>(json);
                return category;
            }
        }

        public async Task<ReadCategoriesPageDto> GetCategoriesAsync(GetCategoriesParamsDto param)
        {
            using (var httpClient = _httpClientFactory.CreateClient("httpClient"))
            {
                var queryString = param.ToQueryString();
                var response = await httpClient.GetAsync("Category" + queryString);

                if (response.StatusCode == HttpStatusCode.Forbidden
                 || response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException();
                else if (!(response.StatusCode == HttpStatusCode.OK))
                    throw new EntityOperationException(EntityType.Category, OperationType.ReadMany);

                var json = await response.Content.ReadAsStringAsync();
                var categoryPage = JsonConvert.DeserializeObject<ReadCategoriesPageDto>(json);
                return categoryPage;
            }
        }

        public async System.Threading.Tasks.Task UpdateCategoryAsync(int id, WriteCategoryDto categoryDto)
        {
            using (var httpClient = _httpClientFactory.CreateClient("httpClient"))
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(categoryDto), Encoding.UTF8, "application/json");

                var response = await httpClient.PutAsync($"Category/{id}", stringContent);

                if (response.StatusCode == HttpStatusCode.Forbidden
                 || response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException();
                else if (!(response.StatusCode == HttpStatusCode.OK))
                    throw new EntityOperationException(EntityType.Category, OperationType.Update);
            }
        }

    }
}
