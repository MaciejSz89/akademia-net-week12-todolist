using System.Collections.Generic;

namespace ToDoList.Services.Navigation
{
    public interface INavigationService
    {
        System.Threading.Tasks.Task GoToPageRelative(string pageName);
        System.Threading.Tasks.Task GoToPageRelative(string pageName, params KeyValuePair<string, string>[] parameters);
        System.Threading.Tasks.Task GoToPageRelative(string pageName, params string[] parameters);

    }
}
