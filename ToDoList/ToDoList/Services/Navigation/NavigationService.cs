using System.Collections.Generic;
using Xamarin.Forms;

namespace ToDoList.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private Shell _currentShell;

        public NavigationService(Shell currentShell)
        {
            _currentShell = currentShell;
        }

        public async System.Threading.Tasks.Task GoToPageRelative(string pageName)
        {
            await _currentShell.GoToAsync(pageName);
        }

        public async System.Threading.Tasks.Task GoToPageRelative(string pageName, params KeyValuePair<string, string>[] parameters)
        {
            var url = $"/{pageName}?";
            var first = true;
            foreach (var parameter in parameters)
            {
                if (!first)
                    url += "&";

                url += $"{parameter.Key}={parameter.Value}";
                first = false;
            }

            await _currentShell.GoToAsync($"{url}");
        }

        public async System.Threading.Tasks.Task GoToPageRelative(string pageName, params string[] parameters)
        {
            var url = $"/{pageName}/";
            foreach (var parameter in parameters)
            {
                url += $"{parameter}/";
            }

            await _currentShell.GoToAsync($"{url}");
        }
    }
}
