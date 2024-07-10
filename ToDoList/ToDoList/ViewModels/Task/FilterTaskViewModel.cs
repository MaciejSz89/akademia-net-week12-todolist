using Android.Views.Inspectors;
using Android.Webkit;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ToDoList.Core;
using ToDoList.Helpers;
using ToDoList.Models.Converters;
using ToDoList.Models.Wrappers.Category;
using ToDoList.Models.Wrappers.Task;
using ToDoList.Services.Category;

namespace ToDoList.ViewModels.Task
{
    public class FilterTaskViewModel : ViewModelBase, IFilterTaskViewModel
    {

        private readonly ITasksViewModel _tasksViewModel;
        private readonly ICategoryService _categoryService;
        private ReadCategoryWrapper? _selectedCategoryFilter;
        private KeyValuePair<string, bool?> _selectedIsExecutedFilter;
        private bool _selectedIsExecutedFilterInitialized;
        private bool _selectedCategoryFilterInitialized;

        public FilterTaskViewModel(ITasksViewModel tasksViewModel,
                                   ICategoryService categoryService)
        {
            _tasksViewModel = tasksViewModel;
            _categoryService = categoryService;
            Categories = new ObservableRangeCollection<ReadCategoryWrapper?>();
            IsExecutedFilters = new ObservableRangeCollection<KeyValuePair<string, bool?>>();
            LoadFiltersCommand = new AsyncCommand(OnLoadFilters);
            ClearFiltersCommand = new Command(OnClearFilters);
            IsBusy = true;
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () => await OnLoadFilters());
            PropertyChanged += OnPropertyChanged;

        }

        private async System.Threading.Tasks.Task OnLoadFilters()
        {

            try
            {
                GetIsExecutedFilter();
                await GetCategoriesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void GetIsExecutedFilter()
        {
            IsExecutedFilters.AddRange(new List<KeyValuePair<string, bool?>>{
                        new KeyValuePair<string, bool?>("---Brak---", null),
                        new KeyValuePair<string, bool?>("Tylko nieukończone", false),
                        new KeyValuePair<string, bool?>("Tylko ukończone", true)
                    });

            SelectedIsExecutedFilter = IsExecutedFilters.FirstOrDefault(x => x.Value == _tasksViewModel.GetTasksParamsWrapper.IsExecuted);
            _selectedIsExecutedFilterInitialized = true;
            OnPropertyChanged(nameof(SelectedIsExecutedFilter));
        }

        private void OnClearFilters()
        {

            try
            {
                GetTasksParamsWrapper.Title = "";
                SelectedIsExecutedFilter = IsExecutedFilters.FirstOrDefault(x => x.Value == null);
                SelectedCategoryFilter = null;
                _tasksViewModel.IsBusy = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedCategoryFilter))
            {
                Debug.WriteLine($"SelectedCategoryFilter changed to {SelectedCategoryFilter?.Name}");
            }
        }

        public GetTasksParamsWrapper GetTasksParamsWrapper => _tasksViewModel.GetTasksParamsWrapper;

        public ObservableRangeCollection<ReadCategoryWrapper?> Categories { get; }

        private async System.Threading.Tasks.Task GetCategoriesAsync()
        {
            var categoryDtos = await _categoryService.GetCategoriesAsync();

            Categories.Add(null);
            Categories.AddRange(categoryDtos.Select(c => c.ToWrapper()));
            var newSelectedCategoryFilter = _tasksViewModel.GetTasksParamsWrapper.CategoryId == null ? null : (await _categoryService.GetCategoryAsync((int)_tasksViewModel.GetTasksParamsWrapper.CategoryId)).ToWrapper();

            SelectedCategoryFilter = null;
            SelectedCategoryFilter = newSelectedCategoryFilter;
            _selectedCategoryFilterInitialized = true;
            OnPropertyChanged(nameof(SelectedCategoryFilter));

        }

        public ReadCategoryWrapper? SelectedCategoryFilter
        {
            get => _selectedCategoryFilter;
            set
            {
                SetProperty(ref _selectedCategoryFilter, value);
                if (!_selectedCategoryFilterInitialized)
                    return;
                GetTasksParamsWrapper.CategoryId = value?.Id;
            }
        }

        public ICommand LoadFiltersCommand { get; }
        public ICommand ClearFiltersCommand { get; }

        public ObservableRangeCollection<KeyValuePair<string, bool?>> IsExecutedFilters { get; }

        public KeyValuePair<string, bool?> SelectedIsExecutedFilter
        {
            get => _selectedIsExecutedFilter;
            set
            {     
                SetProperty(ref _selectedIsExecutedFilter, value);
                if (!_selectedIsExecutedFilterInitialized)
                    return;
                _tasksViewModel.GetTasksParamsWrapper.IsExecuted = value.Value;
            }
        }
    }
}
