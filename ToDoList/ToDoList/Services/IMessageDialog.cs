using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoList.Services
{
    public interface IMessageDialog
    {
        void ShowMessage(string title, string message);
        System.Threading.Tasks.Task ShowMessageAsync(string title, string message);
    }
}
