using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Core;

namespace ToDoList.Helpers
{
    public class CategorySortMethodDescriptionProvider : IEnumDescriptionProvider<CategorySortMethod>
    {
        private readonly Dictionary<CategorySortMethod?, string> _descriptions = new Dictionary<CategorySortMethod?, string>
        {

            { CategorySortMethod.ByIdAscending, "Wg kolejności dodania od najst." },
            { CategorySortMethod.ByIdDescending, "Wg kolejności dodania od najnow." },
            { CategorySortMethod.ByNameAscending, "Wg nazwy alfabetycznie rosnąco" },
            { CategorySortMethod.ByNameDescending, "Wg nazwy alfabetycznie malejąco" }
        };

        public string GetDescription(CategorySortMethod? categorySortMethod)
        {
            if (categorySortMethod == null)
                return "---Brak---";

            return _descriptions.TryGetValue(categorySortMethod, out var description) ? description : categorySortMethod.ToString();
        }

        public IEnumerable<string> GetDescriptions()
        {
            return _descriptions.Values;
        }
    }
}
