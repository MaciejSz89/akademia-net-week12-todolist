using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Dtos;

namespace ToDoList.Services.Category
{
    public interface ICategoryService
    {
        System.Threading.Tasks.Task AddCategoryAsync(WriteCategoryDto taskDto);
        System.Threading.Tasks.Task UpdateCategoryAsync(int id, WriteCategoryDto taskDto);
        System.Threading.Tasks.Task DeleteCategoryAsync(int id);
        Task<ReadCategoryDto> GetCategoryAsync(int id);
        Task<ReadCategoriesPageDto> GetCategoriesAsync(GetCategoriesParamsDto param);
    }
}
