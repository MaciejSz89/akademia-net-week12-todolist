using MvvmHelpers.Commands;
using System.Windows.Input;
using ToDoList.Models.Converters;
using ToDoList.Models.Wrappers.Category;
using ToDoList.Services.Category;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;

namespace ToDoList.ViewModels.Category
{
    public class AddCategoryViewModel : ViewModelBase, IAddCategoryViewModel
    {
        private WriteCategoryWrapper _category = null!;
        private readonly ICategoryService _categoryService;

        public AddCategoryViewModel(ICategoryService categoryService)
        {
            Category = new WriteCategoryWrapper();
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



        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private async void OnCancel()
        {
            await Xamarin.Forms.Shell.Current.GoToAsync("..");
        }

        private async System.Threading.Tasks.Task OnSave()
        {


            await _categoryService.AddCategoryAsync(Category.ToDto());

            await Shell.Current.GoToAsync("..");
        }
    }
}
