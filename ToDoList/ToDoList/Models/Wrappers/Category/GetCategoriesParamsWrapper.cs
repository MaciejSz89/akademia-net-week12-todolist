namespace ToDoList.Models.Wrappers.Category
{
    public class GetCategoriesParamsWrapper
    {

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 8;
    }
}
