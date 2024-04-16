using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using ToDoList.Services.Account;
using ToDoList.Services.Category;
using ToDoList.Services.Task;
using ToDoList.ViewModels.Account;
using ToDoList.ViewModels.Category;
using ToDoList.ViewModels.Task;
using Xamarin.Essentials;

namespace ToDoList
{
    public static class Startup
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static void Init()
        {
            var a = Assembly.GetExecutingAssembly();
            using (var stream = a.GetManifestResourceStream($"{a.GetName().Name}.appsettings.json"))
            {
                var host = new HostBuilder()
                            .ConfigureHostConfiguration(c =>
                            {
                                // Tell the host configuration where to file the file (this is required for Xamarin apps)
                                c.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });

                                //read in the configuration file!
                                c.AddJsonStream(stream);
                            })
                            .ConfigureServices((c, x) =>
                            {
                                // Configure our local services and access the host configuration   
                                ConfigureServices(c, x);
                            })
                            .ConfigureLogging(l => l.AddSimpleConsole(o =>
                            {
                                //setup a console logger and disable colors since they don't have any colors in VS
                                o.ColorBehavior = LoggerColorBehavior.Disabled;
                            }))
                            .Build();

                //Save our service provider so we can use it later.
                ServiceProvider = host.Services;


            }


        }


        public static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            services.AddTransient<IAccountService, AccountService>()
                    .AddTransient<ITaskService, TaskService>()
                    .AddTransient<ICategoryService, CategoryService>()
                    .AddTransient<ILoginViewModel, LoginViewModel>()
                    .AddTransient<IRegisterViewModel, RegisterViewModel>()
                    .AddTransient<IAddTaskViewModel, AddTaskViewModel>()
                    .AddTransient<IEditTaskViewModel, EditTaskViewModel>()
                    .AddTransient<ITasksViewModel, TasksViewModel>()
                    .AddTransient<IViewTaskViewModel, ViewTaskViewModel>()
                    .AddTransient<IAddCategoryViewModel, AddCategoryViewModel>()
                    .AddTransient<IEditCategoryViewModel, EditCategoryViewModel>()
                    .AddTransient<ICategoriesViewModel, CategoriesViewModel>()
                    .AddTransient<IViewCategoryViewModel, ViewCategoryViewModel>()
                    .AddSingleton<App>();
#if DEBUG
            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            services.AddHttpClient("httpClient", client =>
            {
                client.BaseAddress = new Uri(App.BackendUrl);
                var accessToken = Xamarin.Essentials.SecureStorage.GetAsync("AccessToken").Result;

                if(!string.IsNullOrEmpty(accessToken))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            })
            .ConfigurePrimaryHttpMessageHandler(() => httpClientHandler);
#else
            services.AddHttpClient("httpClient", client =>
            {
                client.BaseAddress = new Uri(App.BackendUrl);
            });
#endif

        }


    }
}
