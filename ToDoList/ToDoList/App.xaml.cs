using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
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
        public static string BackendUrl = "http://10.0.2.2:5207/api/";

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();


            Startup.Init();


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

        //private void SetBackendUrl()
        //{
        //    Stream resourceStream = GetType().GetTypeInfo().Assembly.GetManifestResourceStream("ToDoList.appsettings.json");

        //    var configuration = new ConfigurationBuilder()
        //                            .AddJsonStream(resourceStream)
        //                            .Build();

        //    var backendEurl = configuration["BackendUrl"];

        //    if (backendEurl != null)
        //    {
        //        BackendUrl = backendEurl;
        //    }
        //}
    }
}
