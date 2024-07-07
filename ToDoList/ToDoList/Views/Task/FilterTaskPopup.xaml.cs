using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models.Wrappers.Task;
using ToDoList.ViewModels.Task;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDoList.Views.Task
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterTaskPopup : Popup
    {
        public FilterTaskPopup()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService<IFilterTaskViewModel>();
        }

    }
}