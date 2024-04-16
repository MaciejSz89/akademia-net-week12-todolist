using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoList.Models.Wrappers.Task
{
    public class GetCategoriesParamsWrapper
    {
        public bool? IsExecuted { get; set; }
        public int? CategoryId { get; set; }
        public string Title { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 4;
    }
}
