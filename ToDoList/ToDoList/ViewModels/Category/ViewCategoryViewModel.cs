using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ToDoList.Models;
using ToDoList.Models.Converters;
using ToDoList.Models.Wrappers.Category;
using ToDoList.Services.Category;
using ToDoList.Views.Category;
using Xamarin.Forms;

namespace ToDoList.ViewModels.Category
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class ViewCategoryViewModel : BaseViewModel, IViewCategoryViewModel
    {


        private int _id;
        private ReadCategoryWrapper _category;
        private readonly ICategoryService _categoryService;

        public ViewCategoryViewModel(ICategoryService categoryService)
        {
            EditCommand = new Command(OnEdit);
            _categoryService = categoryService;
        }

        private async void OnEdit(object obj)
        {
            var route = $"//{nameof(EditCategoryPage)}?{nameof(EditCategoryViewModel.Id)}={Id}";
            await Shell.Current.GoToAsync($"//{nameof(EditCategoryPage)}?{nameof(EditCategoryViewModel.Id)}={Id}");
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

        public Command EditCommand
        {
            get;
        }

        public async void LoadCategory(int id)
        {
            var categoryDto = await _categoryService.GetCategoryAsync(id);
            Category = categoryDto.ToWrapper();
        }
    }
}
