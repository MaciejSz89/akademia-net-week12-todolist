using ToDoList.Models.Wrappers.Category;
using Xamarin.Forms;

namespace ToDoList.ViewModels.Category
{
    public interface IViewCategoryViewModel : IViewModel
    {
        int Id { get; set; }
        Command EditCommand { get; }
        ReadCategoryWrapper Category { get; set; }

        void LoadCategory(int id);
    }
}