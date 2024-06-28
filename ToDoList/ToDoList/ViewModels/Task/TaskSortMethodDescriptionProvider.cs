using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Core;
using ToDoList.Helpers;

namespace ToDoList.ViewModels.Task
{
    public class TaskSortMethodDescriptionProvider : IEnumDescriptionProvider<TaskSortMethod>
    {
        private readonly Dictionary<TaskSortMethod?, string> _descriptions = new Dictionary<TaskSortMethod?, string>
        {
          
            { TaskSortMethod.ByIdAscending, "Wg kolejności dodania od najst." },
            { TaskSortMethod.ByIdDescending, "Wg kolejności dodania od najnow." },
            { TaskSortMethod.ByTitleAscending, "Wg tytułu alfabetycznie rosnąco" },
            { TaskSortMethod.ByTitleDescending, "Wg tytułu alfabetycznie malejąco" },
            { TaskSortMethod.ByTermAscending, "Wg terminu od najstarszego" },
            { TaskSortMethod.ByTermDescending, "Wg terminu od najnowszego" }
        };

        public string GetDescription(TaskSortMethod? taskSortMethod)
        {
            if (taskSortMethod == null)
                return "===Brak===";

            return _descriptions.TryGetValue(taskSortMethod, out var description) ? description : taskSortMethod.ToString();
        }

        public IEnumerable<string> GetDescriptions()
        {
            return _descriptions.Values;
        }
    }
}
