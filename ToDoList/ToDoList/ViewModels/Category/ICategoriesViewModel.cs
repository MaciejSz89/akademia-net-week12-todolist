using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ToDoList.Models.Wrappers.Category;

namespace ToDoList.ViewModels.Category
{
    public interface ICategoriesViewModel : IViewModel
    {
        int CurrentPage { get; set; }
        GetCategoriesParamsWrapper GetCategoriesParamsWrapper { get; set; }
        int LastPage { get; set; }
        ICommand AddCategoryCommand { get; }
        ICommand LoadCategoriesCommand { get; }
        ICommand DeleteCategoryCommand { get; }
        ICommand NextPageCommand { get; }
        ICommand PreviousPageCommand { get; }
        ICommand CategoryTapped { get; }
        ReadCategoryWrapper? SelectedCategory { get; set; }
        ObservableRangeCollection<ReadCategoryWrapper> Categories { get; }

        void OnAppearing();
    }
}