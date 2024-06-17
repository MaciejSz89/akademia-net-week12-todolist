using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Services.MessageDialog
{
    public interface IMessageDialogService
    {
        void ShowMessage(string title, string message);
        System.Threading.Tasks.Task ShowMessageAsync(string title, string message);
        Task<bool> ShowMessageConfirmAsync(string title, string message);
    }
}
