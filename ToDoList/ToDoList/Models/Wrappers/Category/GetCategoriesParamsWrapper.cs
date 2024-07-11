using ToDoList.Core;
using ToDoList.Helpers;

namespace ToDoList.Models.Wrappers.Category
{
    public class GetCategoriesParamsWrapper : WrapperBase
    {
        private int _pageNumber = 1;
        private int _pageSize = 12;
        private string _sortMethod;

        public GetCategoriesParamsWrapper(IEnumDescriptionProvider<CategorySortMethod> categorySortMethodDescriptionProvider)
        {
            _sortMethod = categorySortMethodDescriptionProvider.GetDescription(CategorySortMethod.ByNameAscending);
        }
        public int PageNumber
        {
            get => _pageNumber;
            set => SetProperty(ref _pageNumber, value);
        }
        public int PageSize
        {
            get => _pageSize;
            set => SetProperty(ref _pageSize, value);
        }

        public string SortMethod
        {
            get => _sortMethod;
            set => SetProperty(ref _sortMethod, value);
        }
    }
}
