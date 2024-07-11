using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ToDoList.Core;
using ToDoList.Helpers;
using ToDoList.Models.Wrappers.Category;

namespace ToDoList.ViewModels.Category
{
    public class SortCategoryViewModel : ViewModelBase, ISortCategoryViewModel
    {
        private readonly ICategoriesViewModel _categoriesViewModel;

        public SortCategoryViewModel(ICategoriesViewModel categoriesViewModel, 
                                     IEnumDescriptionProvider<CategorySortMethod> categorySortMethodDescriptionProvider)
        {
            _categoriesViewModel = categoriesViewModel;
            CategorySortMethods = new ObservableRangeCollection<string>();
            CategorySortMethods.AddRange(categorySortMethodDescriptionProvider.GetDescriptions());
        }
        public ObservableRangeCollection<string> CategorySortMethods { get; }

        public GetCategoriesParamsWrapper GetCategoriesParamsWrapper => _categoriesViewModel.GetCategoriesParamsWrapper;
           

    }
}
