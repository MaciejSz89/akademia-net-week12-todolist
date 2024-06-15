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
using ToDoList.DelegatingHandlers;
using ToDoList.Services;
using ToDoList.Services.Account;
using ToDoList.Services.Category;
using ToDoList.Services.Task;
using ToDoList.ViewModels.Account;
using ToDoList.ViewModels.Category;
using ToDoList.ViewModels.Task;
using Xamarin.Android.Net;
using Xamarin.Essentials;
using Xamarin.Forms;

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
            services.AddSingleton<ILoginViewModel, LoginViewModel>()
                    .AddSingleton<IRegisterViewModel, RegisterViewModel>()
                    .AddSingleton<IAddTaskViewModel, AddTaskViewModel>()
                    .AddSingleton<IEditTaskViewModel, EditTaskViewModel>()
                    .AddSingleton<ITasksViewModel, TasksViewModel>()
                    .AddSingleton<IViewTaskViewModel, ViewTaskViewModel>()
                    .AddSingleton<IAddCategoryViewModel, AddCategoryViewModel>()
                    .AddSingleton<IEditCategoryViewModel, EditCategoryViewModel>()
                    .AddSingleton<ICategoriesViewModel, CategoriesViewModel>()
                    .AddSingleton<IViewCategoryViewModel, ViewCategoryViewModel>()
                    .AddTransient<ErrorsHandler>()
                    .AddTransient<GetCachedAccessTokenHandler>()
                    .AddTransient<IMessageDialog, MessageDialog>(provider=>new MessageDialog(Shell.Current)) 
                    .AddSingleton<App>();

            var httpClientBuilders = new List<IHttpClientBuilder>
            {
                services.AddHttpClient<IAccountService, AccountService>(ConfigureHttpClient),
                services.AddHttpClient<ICategoryService, CategoryService>(ConfigureHttpClient)                        
                        .AddHttpMessageHandler<GetCachedAccessTokenHandler>()
                        .AddHttpMessageHandler<ErrorsHandler>(),
                services.AddHttpClient<ITaskService, TaskService>(ConfigureHttpClient)                        
                        .AddHttpMessageHandler<GetCachedAccessTokenHandler>()
                        .AddHttpMessageHandler<ErrorsHandler>(),
            };


            foreach (var httpClientBuilder in httpClientBuilders)
            {
                httpClientBuilder.ConfigurePrimaryHttpMessageHandler(() =>
                    {
                        // Get platform dependent HttpMessageHandler
                        var handler = Xamarin.Forms.DependencyService.Get<ICustomHttpMessageHandler>().GetHttpMessageHandler();
                       
                        return handler;
                    });

            }

        }


        private static void ConfigureHttpClient(IServiceProvider serviceProvider, HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri(App.BackendUrl);
            httpClient.Timeout = TimeSpan.FromSeconds(120);
        }

    }
}
