using Microsoft.Extensions.DependencyInjection;
using ToDoList.ViewModels.Category;
using Xamarin.Forms;

namespace ToDoList.Views.Category
{
    public partial class ViewCategoryPage : ContentPage
    {
        public ViewCategoryPage()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService<IViewCategoryViewModel>();
        }
    }
}