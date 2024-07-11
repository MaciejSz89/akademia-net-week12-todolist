using MvvmHelpers;
using System.Windows.Input;
using ToDoList.Models.Wrappers.Category;
using ToDoList.Models.Wrappers.Task;

namespace ToDoList.ViewModels.Category
{
    public interface ICategoriesViewModel : IViewModel
    {
        GetCategoriesParamsWrapper GetCategoriesParamsWrapper { get; set; }
        ICommand AddCategoryCommand { get; }
        ICommand LoadCategoriesCommand { get; }
        ICommand DeleteCategoryCommand { get; }
        ICommand EditCategoryCommand { get; }
        ICommand LoadMoreCategoriesCommand { get; }
        ReadCategoryWrapper? SelectedCategory { get; set; }
        ObservableRangeCollection<ReadCategoryWrapper> Categories { get; }
        ICommand CategoryTapped { get; }
        ICommand SelectSortMethodCommand { get; }
        void OnAppearing();
    }
}