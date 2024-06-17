using System;

namespace ToDoList.Models.Wrappers.Task
{
    public class UpdateTaskWrapper : WrapperBase
    {
        private string _title = null!;
        private string _description = null!;
        private DateTime? _term;
        private bool _isExecuted;
        private int _categoryId;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        public DateTime? Term
        {
            get => _term;
            set => SetProperty(ref _term, value);
        }
        public bool IsExecuted
        {
            get => _isExecuted;
            set => SetProperty(ref _isExecuted, value);
        }
        public int CategoryId
        {
            get => _categoryId;
            set => SetProperty(ref _categoryId, value);
        }
    }
}
