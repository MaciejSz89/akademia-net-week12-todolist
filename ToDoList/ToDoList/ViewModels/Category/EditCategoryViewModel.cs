using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models.Converters;
using ToDoList.Models.Wrappers.Category;
using ToDoList.Services.Category;
using Xamarin.Forms;

namespace ToDoList.ViewModels.Category
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class EditCategoryViewModel : BaseViewModel, IEditCategoryViewModel
    {
        private WriteCategoryWrapper _category;
        private int _id;
        private readonly ICategoryService _categoryService;

        public EditCategoryViewModel(ICategoryService categoryService)
        {         
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
            _categoryService = categoryService;
       
        }

        private bool ValidateSave()
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
            Category = (await _categoryService.GetCategoryAsync(id)).ToWriteWrapper();
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            await _categoryService.UpdateCategoryAsync(Id, Category.ToDto());

            await Shell.Current.GoToAsync("../../");
        }
    }
}
