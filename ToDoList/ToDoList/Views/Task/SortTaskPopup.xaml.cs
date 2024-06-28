using System;
using System.Collections.Generic;
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
    public partial class SortTaskPopup : Popup
    {
        public SortTaskPopup()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService<ISortTaskViewModel>();
            picker.Unfocused += ClosePopup;
        }

        private void ClosePopup(object sender, EventArgs e)
        {
            Dismiss(null);
        }
    }
}