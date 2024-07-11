using System;
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
using System.Threading.Tasks;
using ToDoList.Models.Wrappers.Task;
using System.Collections.Generic;
using ToDoList.Core;
using ToDoList.Helpers;
using System.ComponentModel;
using ToDoList.Views.Task;
using Xamarin.CommunityToolkit.Extensions;

namespace ToDoList.ViewModels.Category
{
    public class CategoriesViewModel : ViewModelBase, ICategoriesViewModel
    {
        private ReadCategoryWrapper? _selectedCategory;
        private int _currentPage = 0;
        private int _currentPageSize = 0;
        private GetCategoriesParamsWrapper _getCategoriesParamsWrapper;
        private int _pageSize = 12;
        private readonly ICategoryService _categoryService;
        private IEnumDescriptionProvider<CategorySortMethod> _categorySortMethodDescriptionProvider;


        public CategoriesViewModel(ICategoryService categoryService,
                                   IEnumDescriptionProvider<CategorySortMethod> categorySortMethodDescriptionProvider)
        {
            Title = "Kategorie";
            Categories = new ObservableRangeCollection<ReadCategoryWrapper>();
            LoadCategoriesCommand = new AsyncCommand(OnLoadCategories);
            LoadMoreCategoriesCommand = new AsyncCommand(OnLoadMoreCategories);
            _getCategoriesParamsWrapper = new GetCategoriesParamsWrapper(categorySortMethodDescriptionProvider);

            CategoryTapped = new MvvmHelpers.Commands.Command<ReadCategoryWrapper>(OnCategorySelected);

            AddCategoryCommand = new AsyncCommand(OnAddCategory);
            EditCategoryCommand = new AsyncCommand<ReadCategoryWrapper>(OnEditCategory);
            DeleteCategoryCommand = new AsyncCommand<ReadCategoryWrapper>(OnDeleteCategory);
            SelectSortMethodCommand = new AsyncCommand<GetCategoriesParamsWrapper>(OnSelectSortMethod);
            _categoryService = categoryService;
            _categorySortMethodDescriptionProvider = categorySortMethodDescriptionProvider;
            GetCategoriesParamsWrapper.PropertyChanged += GetCategoriesParamsWrapper_PropertyChanged;

        }


        private void GetCategoriesParamsWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string[] propertiesToCheck = { nameof(GetCategoriesParamsWrapper.PageNumber), nameof(GetCategoriesParamsWrapper.PageSize) };

            if (propertiesToCheck.Contains(e.PropertyName))
                return;

            IsBusy = true;
        }

        public ObservableRangeCollection<ReadCategoryWrapper> Categories { get; }
        public ICommand LoadCategoriesCommand { get; }
        public ICommand LoadMoreCategoriesCommand { get; }

        public ICommand AddCategoryCommand { get; }
        public ICommand EditCategoryCommand { get; }
        public ICommand DeleteCategoryCommand { get; }
        public ICommand CategoryTapped { get; }
        public ICommand SelectSortMethodCommand { get; }
     
        public GetCategoriesParamsWrapper GetCategoriesParamsWrapper
        {
            get => _getCategoriesParamsWrapper;
            set
            {
                SetProperty(ref _getCategoriesParamsWrapper, value);
            }
        }

        private async System.Threading.Tasks.Task OnAddCategory()
        {
            await Shell.Current.GoToAsync($"{nameof(AddCategoryPage)}");
        }
        private async System.Threading.Tasks.Task OnEditCategory(ReadCategoryWrapper categoryWrapper)
        {
            if (categoryWrapper == null)
                return;

            var route = $"/{nameof(EditCategoryPage)}?{nameof(ReadCategoryWrapper.Id)}={categoryWrapper.Id}";
            await Shell.Current.GoToAsync(route);
        }
        private async System.Threading.Tasks.Task OnDeleteCategory(ReadCategoryWrapper categoryWrapper)
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

        private async System.Threading.Tasks.Task OnLoadCategories()
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

        private async System.Threading.Tasks.Task OnLoadMoreCategories()
        {
            try
            {
                await LoadMoreCategories();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async System.Threading.Tasks.Task LoadMoreCategories()
        {
            if (IsBusy || _currentPage == 0)
                return;

            GetCategoriesParamsWrapper.PageSize = _pageSize;
            GetCategoriesParamsWrapper.PageNumber = _pageSize == _currentPageSize ? _currentPage + 1 : _currentPage;

            var categoriesPage = await _categoryService.GetCategoriesAsync(GetCategoriesParamsWrapper.ToDto());


            if (!categoriesPage.Categories.Any()
             || GetCategoriesParamsWrapper.PageNumber != categoriesPage.CurrentPage)
                return;


            var fetchedPageSize = categoriesPage.Categories.Count();
            var categories = categoriesPage.Categories
                                 .Select(x => x.ToWrapper());

            var newCategoriesForCurrentPage = categoriesPage.CurrentPage == _currentPage
                                      && fetchedPageSize > _currentPageSize;

            var nextPage = categoriesPage.CurrentPage == _currentPage + 1;

            if (newCategoriesForCurrentPage)
            {
                categories = categories.Skip(_currentPageSize)
                             .Take(fetchedPageSize - _currentPageSize);

                _currentPageSize += categories.Count();
                Categories.AddRange(categories);
                SortCategories();
                return;
            }

            if (nextPage)
            {
                _currentPageSize = categories.Count();
                _currentPage += 1;
                Categories.AddRange(categories);
                SortCategories();
                return;
            }
        }

        private async System.Threading.Tasks.Task LoadCategories()
        {
            Categories.Clear();
            GetCategoriesParamsWrapper.PageSize = _pageSize;
            GetCategoriesParamsWrapper.PageNumber = 1;
            var categoriesPage = await _categoryService.GetCategoriesAsync(GetCategoriesParamsWrapper.ToDto());
            if (categoriesPage.Categories.Any())
            {

                var categories = categoriesPage.Categories
                                     .Select(x => x.ToWrapper());

                _currentPage = 1;
                _currentPageSize = categories.Count();

                Categories.ReplaceRange(categories);

                SortCategories();
            }
        }


        public void OnAppearing()
        {
            IsBusy = true;
            SelectedCategory = null;
        }

        public ReadCategoryWrapper? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                SetProperty(ref _selectedCategory, value);
                OnCategorySelected(value);
            }
        }


        async void OnCategorySelected(ReadCategoryWrapper? category)
        {
            if (category == null)
                return;
            var route = $"/{nameof(ViewCategoryPage)}?{nameof(ViewCategoryViewModel.Id)}={category.Id}";
            await Shell.Current.GoToAsync(route);
        }

        private void SortCategories()
        {

            var categories = new List<ReadCategoryWrapper>(Categories);
            var selectedCategorySortMethod = EnumHelper.GetEnumFromDescription<CategorySortMethod>(GetCategoriesParamsWrapper.SortMethod, _categorySortMethodDescriptionProvider);

            switch (selectedCategorySortMethod)
            {
                case CategorySortMethod.ByIdAscending:
                    categories = Categories.OrderBy(x => x.Id)
                                           .ToList();
                    break;
                case CategorySortMethod.ByIdDescending:
                    categories = Categories.OrderByDescending(x => x.Id)
                                           .ToList();
                    break;
                case CategorySortMethod.ByNameAscending:
                    categories = Categories.OrderBy(x => x.Name)
                                           .ToList();
                    break;
                case CategorySortMethod.ByNameDescending:
                    categories = Categories.OrderByDescending(x => x.Name)
                                           .ToList();
                    break;
                default:
                    break;
            }
            Categories.ReplaceRange(categories);


        }

        private async System.Threading.Tasks.Task OnSelectSortMethod(GetCategoriesParamsWrapper param)
        {
#pragma warning disable CS8620 
            var result = await App.Current.MainPage.ShowPopupAsync(new SortCategoryPopup());
#pragma warning restore CS8620 
        }
    }
}