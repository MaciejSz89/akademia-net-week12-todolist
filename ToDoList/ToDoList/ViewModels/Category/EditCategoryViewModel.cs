using MvvmHelpers.Commands;
using System.Windows.Input;
using ToDoList.Models.Converters;
using ToDoList.Models.Wrappers.Category;
using ToDoList.Services.Category;
using ToDoList.Views.Category;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;

namespace ToDoList.ViewModels.Category
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class EditCategoryViewModel : ViewModelBase, IEditCategoryViewModel
    {
        private WriteCategoryWrapper _category = null!;
        private int _id;
        private readonly ICategoryService _categoryService;

        public EditCategoryViewModel(ICategoryService categoryService)
        {         
            SaveCommand = new AsyncCommand(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            PropertyChanged +=
                (_, __) => (SaveCommand as AsyncCommand)?.RaiseCanExecuteChanged();
            _categoryService = categoryService;
       
        }

        private bool ValidateSave(object obj)
        {
            //TODO: Add validation
            return true;
        }

        public WriteCategoryWrapper Category
        {
            get => _category;
            set
            {
                SetProperty(ref _category, value);
            }
        }

        public int Id
        {
            get => _id;
            set
            {
                SetProperty(ref _id, value);
                LoadTask(value);
            }
        }

        private async void LoadTask(int id)
        {
            Category = (await _categoryService.GetCategoryAsync(id))?.ToWriteWrapper()!;
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private async void OnCancel()
        {
            await Xamarin.Forms.Shell.Current.GoToAsync($"//{nameof(CategoriesPage)}");
        }

        private async System.Threading.Tasks.Task OnSave()
        {
            await _categoryService.UpdateCategoryAsync(Id, Category.ToDto());

            await Xamarin.Forms.Shell.Current.GoToAsync($"//{nameof(CategoriesPage)}");
        }
    }
}
