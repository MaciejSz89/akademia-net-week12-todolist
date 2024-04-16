using ToDoList.ViewModels.Category;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDoList.Views.Category
{
    public partial class CategoriesPage : ContentPage
    {
        ICategoriesViewModel _viewModel;

        public CategoriesPage()
        {
            InitializeComponent();
            _viewModel = Startup.ServiceProvider.GetService<ICategoriesViewModel>();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}