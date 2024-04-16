using System.Collections.ObjectModel;
using ToDoList.Models.Wrappers.Category;
using Xamarin.Forms;

namespace ToDoList.ViewModels.Category
{
    public interface ICategoriesViewModel : IViewModel
    {
        Command AddCategoryCommand { get; }
        int CurrentPage { get; set; }
        Command DeleteCategoryCommand { get; }
        GetCategoriesParamsWrapper GetCategoriesParamsWrapper { get; set; }
        int LastPage { get; set; }
        Command LoadCategoriesCommand { get; }
        Command NextPageCommand { get; }
        Command PreviousPageCommand { get; }
        ReadCategoryWrapper SelectedCategory { get; set; }
        ObservableCollection<ReadCategoryWrapper> Categories { get; }
        Command<ReadCategoryWrapper> CategoryTapped { get; }

        void OnAppearing();
    }
}