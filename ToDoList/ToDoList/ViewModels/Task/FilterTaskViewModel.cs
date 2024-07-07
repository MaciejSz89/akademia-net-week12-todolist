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

        public FilterTaskViewModel(ITasksViewModel tasksViewModel,
                                   ICategoryService categoryService)
        {
            _tasksViewModel = tasksViewModel;
            _categoryService = categoryService;
            Categories = new ObservableRangeCollection<ReadCategoryWrapper?>();
            LoadFiltersCommand = new Command(async () => await OnLoadFiltersCommand());
            IsBusy = true;
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async() => await OnLoadFiltersCommand());
            PropertyChanged += OnPropertyChanged;

        }

        private async System.Threading.Tasks.Task OnLoadFiltersCommand()
        {
            IsBusy = true;

            try
            {
                await GetCategoriesAsync();
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
            IsBusy = true;
            var categoryDtos = await _categoryService.GetCategoriesAsync();

            Categories.Add(null);
            Categories.AddRange(categoryDtos.Select(c => c.ToWrapper()));
            var newSelectedCategoryFilter = _tasksViewModel.GetTasksParamsWrapper.CategoryId == null ? null : (await _categoryService.GetCategoryAsync((int)_tasksViewModel.GetTasksParamsWrapper.CategoryId)).ToWrapper();

            SelectedCategoryFilter = null;
            SelectedCategoryFilter = newSelectedCategoryFilter;
            OnPropertyChanged(nameof(SelectedCategoryFilter));
            IsBusy = false;

        }

        public ReadCategoryWrapper? SelectedCategoryFilter
        {
            get => _selectedCategoryFilter;
            set
            {
                SetProperty(ref _selectedCategoryFilter, value);
                GetTasksParamsWrapper.CategoryId = value?.Id;
            }
        }

        public Command LoadFiltersCommand { get; }
    }
}
