using MvvmHelpers.Commands;
using ToDoList.Models.Wrappers.Task;

namespace ToDoList.ViewModels.Task
{
    public interface IViewTaskViewModel : IViewModel
    {
        int Id { get; set; }
        ReadTaskWrapper Task { get; set; }

        void LoadTask(int id);
        Command UpdateIsExecutedCommand { get; }
    }
}