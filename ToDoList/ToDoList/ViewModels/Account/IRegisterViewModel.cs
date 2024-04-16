using System.ComponentModel;
using ToDoList.Models.Wrappers.Account;
using Xamarin.Forms;

namespace ToDoList.ViewModels.Account
{
    public interface IRegisterViewModel : INotifyPropertyChanged
    {
        Command LoginCommand { get; }
        Command RegisterCommand { get; }
        RegisterWrapper RegisterUser { get; set; }
    }
}