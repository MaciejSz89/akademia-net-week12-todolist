using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models.Wrappers.Category;
using ToDoList.ViewModels.Category;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDoList.Views.Category
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SortCategoryPopup : Popup
    {
        public SortCategoryPopup()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService<ISortCategoryViewModel>();
            picker.Unfocused += ClosePopup;
        }

        private void ClosePopup(object sender, EventArgs e)
        {
            Dismiss(null);
        }
    }
}