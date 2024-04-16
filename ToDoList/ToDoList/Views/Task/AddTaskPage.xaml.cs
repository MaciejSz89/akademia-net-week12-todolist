using ToDoList.ViewModels.Task;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDoList.Views.Task
{
    public partial class AddTaskPage : ContentPage
    {
        public AddTaskPage()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService<IAddTaskViewModel>();
        }
    }
}