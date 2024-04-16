using ToDoList.Models.Wrappers.Category;
using Xamarin.Forms;

namespace ToDoList.ViewModels.Category
{
    public interface IEditCategoryViewModel : IViewModel
    {
        Command CancelCommand { get; }
        int Id { get; set; }
        Command SaveCommand { get; }
        WriteCategoryWrapper Category { get; set; }
    }
}