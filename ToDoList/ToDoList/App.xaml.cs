using System;
using System.Net.Http;
using ToDoList.Services;
using ToDoList.Services.Account;
using ToDoList.ViewModels.Account;
using ToDoList.Views;
using ToDoList.Views.Account;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDoList
{
    public partial class App : Application
    {
        public static string BackendUrl = "https://10.0.2.2:7021/api/";

        public App()
        {
            InitializeComponent();

            Startup.Init();

            MainPage = new AppShell();
            Shell.Current.GoToAsync("//LoginPage");


        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
