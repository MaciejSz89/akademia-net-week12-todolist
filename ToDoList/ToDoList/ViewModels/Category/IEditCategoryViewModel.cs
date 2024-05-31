using System.Windows.Input;
using ToDoList.Models.Wrappers.Category;

namespace ToDoList.ViewModels.Category
{
    public interface IEditCategoryViewModel : IViewModel
    {
        int Id { get; set; }
        ICommand SaveCommand { get; }
        ICommand CancelCommand { get; }
        WriteCategoryWrapper Category { get; set; }
    }
}