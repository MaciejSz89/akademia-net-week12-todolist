using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Core;
using ToDoList.Helpers;

namespace ToDoList.ViewModels.Task
{
    public class TaskSortMethodDescriptionProvider : IEnumDescriptionProvider<TaskSortMethod>
    {
        public readonly Dictionary<TaskSortMethod?, string> Descriptions = new Dictionary<TaskSortMethod?, string>
        {
            { null, "===Brak===" },
            { TaskSortMethod.ByIdAscending, "Według kolejności dodania od najstarszego" },
            { TaskSortMethod.ByIdDescending, "Według kolejności dodania od najnowszego" },
            { TaskSortMethod.ByTitleAscending, "Według tytułu alfabetycznie rosnąco" },
            { TaskSortMethod.ByTitleDescending, "Według tytułu alfabetycznie malejąco" },
            { TaskSortMethod.ByTermAscending, "Według terminu od najstarszego" },
            { TaskSortMethod.ByTermDescending, "Według terminu od najnowszego" }
        };

        public string GetDescription(TaskSortMethod? value)
        {
            return Descriptions.TryGetValue(value, out var description) ? description : value.ToString();
        }
    }
}
