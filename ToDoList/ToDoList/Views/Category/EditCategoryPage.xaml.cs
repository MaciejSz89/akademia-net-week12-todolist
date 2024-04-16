using ToDoList.ViewModels;
using ToDoList.ViewModels.Category;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDoList.Views.Category
{
    public partial class EditCategoryPage : ContentPage
    {
        public EditCategoryPage()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService<IEditCategoryViewModel>();
        }

    }
}