using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;
using ToDoList.Models.Wrappers.Category;
using ToDoList.Services.Category;
using ToDoList.Models.Converters;
using System.Linq;
using ToDoList.Views.Category;

namespace ToDoList.ViewModels.Category
{
    public class CategoriesViewModel : BaseViewModel, ICategoriesViewModel
    {
        private ReadCategoryWrapper _selectedCategory;
        private int _currentPage = 1;
        private int _lastPage = 1;
        private GetCategoriesParamsWrapper _getCategoriesParamsWrapper;
        private readonly ICategoryService _categoryService;

        public CategoriesViewModel(ICategoryService categoryService)
        {
            Title = "Kategorie";
            Categories = new ObservableCollection<ReadCategoryWrapper>();
            LoadCategoriesCommand = new Command(async () => await ExecuteLoadCategoriesCommand());
            GetCategoriesParamsWrapper = new GetCategoriesParamsWrapper();

            CategoryTapped = new Command<ReadCategoryWrapper>(OnCategorySelected);

            AddCategoryCommand = new Command(OnAddCategory);
            EditCategoryCommand = new Command<ReadCategoryWrapper>(async (x) => await OnEditCategory(x));
            DeleteCategoryCommand = new Command<ReadCategoryWrapper>(async (x) => await OnDeleteCategory(x));
            PreviousPageCommand = new Command(async (x) => await ExecutePreviousPageCommand(), ValidatePreviousPage);
            NextPageCommand = new Command(async (x) => await ExecuteNextPageCommand(), ValidateNextPage);
            _categoryService = categoryService;
        }


        public ObservableCollection<ReadCategoryWrapper> Categories { get; }
        public Command LoadCategoriesCommand { get; }

        public Command AddCategoryCommand { get; }
        public Command EditCategoryCommand { get; }
        public Command DeleteCategoryCommand { get; }
        public Command PreviousPageCommand { get; }
        public Command NextPageCommand { get; }
        public Command<ReadCategoryWrapper> CategoryTapped { get; }
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                SetProperty(ref _currentPage, value);
                GetCategoriesParamsWrapper.PageNumber = value;
                PreviousPageCommand.ChangeCanExecute();
                NextPageCommand.ChangeCanExecute();
            }
        }
        public int LastPage
        {
            get => _lastPage;
            set
            {
                SetProperty(ref _lastPage, value);
                NextPageCommand.ChangeCanExecute();
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
        private async System.Threading.Tasks.Task OnEditCategory(ReadCategoryWrapper categoryWrapper)
        {

            await Shell.Current.GoToAsync($"//{nameof(EditCategoryPage)}?Id={categoryWrapper.Id}");
        }
        private async System.Threading.Tasks.Task OnDeleteCategory(ReadCategoryWrapper categoryWrapper)
        {
            if (categoryWrapper == null)
                return;

            var dialog = await Shell.Current.DisplayAlert("Usuwanie!", $"Czy na pewno chcesz usunąć kategorię: {categoryWrapper.Id}", "Tak", "Nie");

            if (!dialog)
                return;

            await _categoryService.DeleteCategoryAsync(categoryWrapper.Id);

            await ExecuteLoadCategoriesCommand();
        }

        private async System.Threading.Tasks.Task ExecuteLoadCategoriesCommand()
        {
            IsBusy = true;

            try
            {
                Categories.Clear();
                var categoriesPage = await _categoryService.GetCategoriesAsync(GetCategoriesParamsWrapper.ToDto());
                var categories = categoriesPage.Categories.Select(x => x.ToWrapper());

                foreach (var category in categories)
                {
                    Categories.Add(category);
                }
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

        private async void OnAddCategory(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(AddCategoryPage)}");
        }

        async void OnCategorySelected(ReadCategoryWrapper category)
        {
            if (category == null)
                return;
            var route = $"{nameof(ViewCategoryPage)}?{nameof(ViewCategoryViewModel.Id)}={category.Id}";
            await Shell.Current.GoToAsync(route);
        }
    }
}