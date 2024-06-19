using MvvmHelpers.Commands;
using System.Windows.Input;
using ToDoList.Models.Converters;
using ToDoList.Models.Wrappers.Category;
using ToDoList.Services.Category;
using ToDoList.Views.Category;

namespace ToDoList.ViewModels.Category
{
    [Xamarin.Forms.QueryProperty(nameof(Id), nameof(Id))]
    public class ViewCategoryViewModel : ViewModelBase, IViewCategoryViewModel
    {


        private int _id;
        private ReadCategoryWrapper _category = null!;
        private readonly ICategoryService _categoryService;

        public ViewCategoryViewModel(ICategoryService categoryService)
        {
            EditCommand = new AsyncCommand(OnEdit);
            _categoryService = categoryService;
        }

        private async System.Threading.Tasks.Task OnEdit()
        {
            var route = $"/{nameof(EditCategoryPage)}?{nameof(EditCategoryViewModel.Id)}={Id}";
            await Xamarin.Forms.Shell.Current.GoToAsync(route);
        }

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                LoadCategory(value);
            }
        }

        public ReadCategoryWrapper Category
        {
            get => _category;
            set
            {
                SetProperty(ref _category, value);
            }
        }

        public ICommand EditCommand
        {
            get;
        }

        public async void LoadCategory(int id)
        {
            var categoryDto = await _categoryService.GetCategoryAsync(id);
            Category = categoryDto!.ToWrapper();
        }
    }
}
