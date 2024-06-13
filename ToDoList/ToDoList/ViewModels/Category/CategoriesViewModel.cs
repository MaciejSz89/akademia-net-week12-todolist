using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using ToDoList.Models.Wrappers.Category;
using ToDoList.Services.Category;
using ToDoList.Models.Converters;
using System.Linq;
using ToDoList.Views.Category;
using MvvmHelpers.Commands;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;
using System.Windows.Input;
using MvvmHelpers;

namespace ToDoList.ViewModels.Category
{
    public class CategoriesViewModel : ViewModelBase, ICategoriesViewModel
    {
        private ReadCategoryWrapper _selectedCategory;
        private int _currentPage = 1;
        private int _lastPage = 1;
        private GetCategoriesParamsWrapper _getCategoriesParamsWrapper;
        private readonly ICategoryService _categoryService;


        public CategoriesViewModel(ICategoryService categoryService)
        {
            Title = "Kategorie";
            Categories = new ObservableRangeCollection<ReadCategoryWrapper>();
            LoadCategoriesCommand = new AsyncCommand(() => ExecuteLoadCategoriesCommand());
            GetCategoriesParamsWrapper = new GetCategoriesParamsWrapper();

            CategoryTapped = new MvvmHelpers.Commands.Command<ReadCategoryWrapper>(OnCategorySelected);

            AddCategoryCommand = new Command(ExecuteAddCategoryCommand);
            EditCategoryCommand = new MvvmHelpers.Commands.Command<ReadCategoryWrapper>(async (x) => await ExecuteEditCategoryCommand(x));
            DeleteCategoryCommand = new MvvmHelpers.Commands.Command<ReadCategoryWrapper>(async (x) => await ExecuteDeleteCategoryCommand(x));
            PreviousPageCommand = new Command(async (x) => await ExecutePreviousPageCommand(), ValidatePreviousPage);
            NextPageCommand = new Command(async (x) => await ExecuteNextPageCommand(), ValidateNextPage);
            _categoryService = categoryService;
        }


        public ObservableRangeCollection<ReadCategoryWrapper> Categories { get; }
        public ICommand LoadCategoriesCommand { get; }

        public ICommand AddCategoryCommand { get; }
        public ICommand EditCategoryCommand { get; }
        public ICommand DeleteCategoryCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand CategoryTapped { get; }
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                SetProperty(ref _currentPage, value);
                GetCategoriesParamsWrapper.PageNumber = value;
                (PreviousPageCommand as Command).RaiseCanExecuteChanged();
                (NextPageCommand as Command).RaiseCanExecuteChanged();
            }
        }
        public int LastPage
        {
            get => _lastPage;
            set
            {
                SetProperty(ref _lastPage, value);
                (NextPageCommand as Command).RaiseCanExecuteChanged();
            }
        }
        public GetCategoriesParamsWrapper GetCategoriesParamsWrapper
        {
            get => _getCategoriesParamsWrapper;
            set
            {
                SetProperty(ref _getCategoriesParamsWrapper, value);
            }
        }

        private bool ValidateNextPage(object arg)
        {
            return CurrentPage != LastPage;
        }

        private bool ValidatePreviousPage(object arg)
        {
            return CurrentPage != 1;
        }

        private async System.Threading.Tasks.Task ExecuteNextPageCommand()
        {
            IsBusy = true;
            CurrentPage += 1;
            await ExecuteLoadCategoriesCommand();
        }

        private async System.Threading.Tasks.Task ExecutePreviousPageCommand()
        {
            IsBusy = true;
            CurrentPage -= 1;
            await ExecuteLoadCategoriesCommand();
        }
        private async System.Threading.Tasks.Task ExecuteEditCategoryCommand(ReadCategoryWrapper categoryWrapper)
        {
            if (categoryWrapper == null)
                return;

            var route = $"/{nameof(EditCategoryPage)}?{nameof(ReadCategoryWrapper.Id)}={categoryWrapper.Id}";
            await Shell.Current.GoToAsync(route);
        }
        private async System.Threading.Tasks.Task ExecuteDeleteCategoryCommand(ReadCategoryWrapper categoryWrapper)
        {
            IsBusy = true;
            if (categoryWrapper == null)
                return;

            var dialog = await Shell.Current.DisplayAlert("Usuwanie!", $"Czy na pewno chcesz usunąć kategorię: {categoryWrapper.Id}", "Tak", "Nie");

            if (!dialog)
                return;

            await _categoryService.DeleteCategoryAsync(categoryWrapper.Id);


            await LoadCategories();
            IsBusy = false;

        }

        private async System.Threading.Tasks.Task ExecuteLoadCategoriesCommand()
        {
            IsBusy = true;

            try
            {
                await LoadCategories();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async System.Threading.Tasks.Task LoadCategories()
        {

            var categoriesPage = await _categoryService.GetCategoriesAsync(GetCategoriesParamsWrapper.ToDto());
            var categories = categoriesPage.Categories
                                           .OrderBy(x => x.Id)
                                           .Select(x => x.ToWrapper())
                                           .ToList();

            Categories.ReplaceRange(categories);
        }



        public void OnAppearing()
        {
            IsBusy = true;
            SelectedCategory = null;
        }

        public ReadCategoryWrapper SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                SetProperty(ref _selectedCategory, value);
                OnCategorySelected(value);
            }
        }

        private async void ExecuteAddCategoryCommand(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(AddCategoryPage)}");
        }

        async void OnCategorySelected(ReadCategoryWrapper category)
        {
            if (category == null)
                return;
            var route = $"/{nameof(ViewCategoryPage)}?{nameof(ViewCategoryViewModel.Id)}={category.Id}";
            await Shell.Current.GoToAsync(route);
        }
    }
}