using Microsoft.Extensions.DependencyInjection;
using ToDoList.ViewModels.Task;
using Xamarin.Forms;

namespace ToDoList.Views.Task
{
    public partial class ViewTaskPage : ContentPage
    {
        public ViewTaskPage()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService<IViewTaskViewModel>();
        }
    }
}