using MvvmHelpers.Commands;
using System.Windows.Input;
using ToDoList.Models.Wrappers.Task;

namespace ToDoList.ViewModels.Task
{
    public interface IViewTaskViewModel : IViewModel
    {
        int Id { get; set; }
        ReadTaskWrapper Task { get; set; }

        void LoadTask(int id);
        ICommand UpdateIsExecutedCommand { get; }
        ICommand EditCommand { get; }
    }
}