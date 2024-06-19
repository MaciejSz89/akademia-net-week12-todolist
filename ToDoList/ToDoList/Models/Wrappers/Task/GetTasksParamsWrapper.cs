using ToDoList.Core;

namespace ToDoList.Models.Wrappers.Task
{
    public class GetCategoriesParamsWrapper : WrapperBase
    {
        private bool? _isExecuted;
        private int? _categoryId;
        private string _title = null!;
        private int _pageNumber = 1;
        private int _pageSize = 4;
        private TaskSortMethod _sortMethod;

        public bool? IsExecuted
        {
            get => _isExecuted;
            set => SetProperty(ref _isExecuted, value);
        }
        public int? CategoryId
        {
            get => _categoryId;
            set => SetProperty(ref _categoryId, value);
        }
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
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

        public TaskSortMethod SortMethod
        {
            get => _sortMethod;
            set => SetProperty(ref _sortMethod, value);
        }
    }
}
