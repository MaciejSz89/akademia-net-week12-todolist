using ToDoList.ViewModels.Category;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDoList.Views.Task
{
    public partial class EditTaskPage : ContentPage
    {

        public EditTaskPage()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService<IEditCategoryViewModel>();
        }
    }
}