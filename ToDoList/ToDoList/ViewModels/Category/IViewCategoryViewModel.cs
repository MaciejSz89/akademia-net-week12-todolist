using System.Windows.Input;
using ToDoList.Models.Wrappers.Category;

namespace ToDoList.ViewModels.Category
{
    public interface IViewCategoryViewModel : IViewModel
    {
        int Id { get; set; }
        ICommand EditCommand { get; }
        ReadCategoryWrapper Category { get; set; }

        void LoadCategory(int id);
    }
}