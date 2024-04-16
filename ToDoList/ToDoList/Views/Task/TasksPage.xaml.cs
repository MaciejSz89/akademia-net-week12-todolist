using ToDoList.ViewModels.Task;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDoList.Views.Task
{
    public partial class TasksPage : ContentPage
    {
        private readonly ITasksViewModel _viewModel;

        public TasksPage()
        {
            InitializeComponent();
            _viewModel = Startup.ServiceProvider.GetService<ITasksViewModel>();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}