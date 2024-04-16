using System.ComponentModel;
using ToDoList.Models.Wrappers.Account;
using Xamarin.Forms;

namespace ToDoList.ViewModels.Account
{
    public interface ILoginViewModel: INotifyPropertyChanged
    {
        Command LoginCommand { get; }
        Command RegisterCommand { get; }
        LoginWrapper Login { get; set; }
    }
}