using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.ViewModels;
using ToDoList.ViewModels.Account;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDoList.Views.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {

        public LoginPage()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService<ILoginViewModel>();
        }
    }
}