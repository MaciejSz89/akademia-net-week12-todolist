using System.Windows.Input;
using ToDoList.Models.Wrappers.Category;

namespace ToDoList.ViewModels.Category
{
    public interface IAddCategoryViewModel : IViewModel
    {
        ICommand CancelCommand { get; }
        ICommand SaveCommand { get; }
        WriteCategoryWrapper Category { get; set; }
    }
}