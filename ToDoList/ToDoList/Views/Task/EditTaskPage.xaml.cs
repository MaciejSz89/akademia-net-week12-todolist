using ToDoList.ViewModels.Category;
using ToDoList.ViewModels.Task;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDoList.Views.Task
{
    public partial class EditTaskPage : ContentPage
    {

        public EditTaskPage()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService<IEditTaskViewModel>();
        }
    }
}