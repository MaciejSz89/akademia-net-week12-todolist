using System;
using System.Collections.Generic;
using ToDoList.ViewModels;
using ToDoList.Views;
using ToDoList.Views.Account;
using ToDoList.Views.Category;
using ToDoList.Views.Task;
using Xamarin.Forms;

namespace ToDoList
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AddCategoryPage), typeof(AddCategoryPage));
            Routing.RegisterRoute(nameof(CategoriesPage), typeof(CategoriesPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
