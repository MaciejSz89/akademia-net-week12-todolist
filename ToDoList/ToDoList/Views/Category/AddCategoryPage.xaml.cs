using ToDoList.ViewModels.Category;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDoList.Views.Category
{
    public partial class AddCategoryPage : ContentPage
    {
        public AddCategoryPage()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService<IAddCategoryViewModel>();
        }
    }
}