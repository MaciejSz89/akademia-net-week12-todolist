using ToDoList.Models.Wrappers.Category;
using Xamarin.Forms;

namespace ToDoList.ViewModels.Category
{
    public interface IAddCategoryViewModel : IViewModel
    {
        Command CancelCommand { get; }
        Command SaveCommand { get; }
        WriteCategoryWrapper Category { get; set; }
    }
}