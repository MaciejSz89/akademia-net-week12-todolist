using System.ComponentModel;

namespace ToDoList.ViewModels
{
    public interface IViewModel
    {
        bool IsBusy { get; set; }
        string Title { get; set; }

        event PropertyChangedEventHandler PropertyChanged;
    }
}