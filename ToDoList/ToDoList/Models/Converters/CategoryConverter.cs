using ToDoList.Core.Dtos;
using ToDoList.Models.Wrappers.Category;

namespace ToDoList.Models.Converters
{
    public static class CategoryConverter
    {
        public static GetCategoriesParamsDto ToDto(this GetCategoriesParamsWrapper wrapper)
        {
            var getCategoriesParamsDto = new GetCategoriesParamsDto
            {
                PageNumber = wrapper.PageNumber,
                PageSize = wrapper.PageSize
            };
            return getCategoriesParamsDto;
        }


        public static ReadCategoryWrapper ToWrapper(this ReadCategoryDto dto)
        {
            var readCategoryWrapper = new ReadCategoryWrapper
            {
                Id = dto.Id,
                Name = dto.Name
            };
            return readCategoryWrapper;
        }

        public static WriteCategoryDto ToDto(this WriteCategoryWrapper wrapper)
        {
            var writeCategoryDto = new WriteCategoryDto
            {
                Name = wrapper.Name
            };
            return writeCategoryDto;
        }


        public static WriteCategoryWrapper ToWriteWrapper(this ReadCategoryDto dto)
        {
            var writeCategoryWrapper = new WriteCategoryWrapper
            {
                Name = dto.Name
            };
            return writeCategoryWrapper;
        }
    }
}
