using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ToDoList.Models;
using ToDoList.Models.Converters;
using ToDoList.Models.Wrappers.Category;
using ToDoList.Services.Category;
using Xamarin.Forms;

namespace ToDoList.ViewModels.Category
{
    public class AddCategoryViewModel : BaseViewModel, IAddCategoryViewModel
    {
        private WriteCategoryWrapper _category;
        private readonly ICategoryService _categoryService;

        public AddCategoryViewModel(ICategoryService categoryService)
        {
            Category = new WriteCategoryWrapper();
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



        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {


            await _categoryService.AddCategoryAsync(Category.ToDto());

            await Shell.Current.GoToAsync("..");
        }
    }
}
