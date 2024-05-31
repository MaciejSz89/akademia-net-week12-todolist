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
using ToDoList.Models.Converters;
using ToDoList.Models.Wrappers.Category;
using Xamarin.Forms;

namespace ToDoList.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async System.Threading.Tasks.Task AddCategoryAsync(WriteCategoryDto categoryDto)
        {

            var stringContent = new StringContent(JsonConvert.SerializeObject(categoryDto), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("Category", stringContent);

        }

        public async System.Threading.Tasks.Task DeleteCategoryAsync(int id)
        {

            var response = await _httpClient.DeleteAsync($"Category/{id}");

        }

        public async Task<ReadCategoryDto> GetCategoryAsync(int id)
        {
            var response = await _httpClient.GetAsync($"Category/{id}");


            var json = await response.Content.ReadAsStringAsync();
            var category = JsonConvert.DeserializeObject<ReadCategoryDto>(json);
            return category;
        }

        public async Task<IEnumerable<ReadCategoryDto>> GetCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("Category");

            var json = await response.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<IEnumerable<ReadCategoryDto>>(json);
            return categories;
        }


        public async Task<ReadCategoriesPageDto> GetCategoriesAsync(GetCategoriesParamsDto param)
        {
            var queryString = param.ToQueryString();
            var response = await _httpClient.GetAsync("Category" + queryString);

            var json = await response.Content.ReadAsStringAsync();
            var categoryPage = JsonConvert.DeserializeObject<ReadCategoriesPageDto>(json);
            return categoryPage;
        }




        public async System.Threading.Tasks.Task UpdateCategoryAsync(int id, WriteCategoryDto categoryDto)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(categoryDto), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"Category/{id}", stringContent);
        }

    }
}
