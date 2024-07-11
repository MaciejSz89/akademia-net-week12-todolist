using MvvmHelpers;
using ToDoList.Models.Wrappers.Category;

namespace ToDoList.ViewModels.Category
{
    public interface ISortCategoryViewModel : IViewModel
    {
        ObservableRangeCollection<string> CategorySortMethods { get; }
        GetCategoriesParamsWrapper GetCategoriesParamsWrapper { get; }

    }
}
