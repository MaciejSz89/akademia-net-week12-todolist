namespace ToDoList.Models.Wrappers.Category
{
    public class WriteCategoryWrapper : WrapperBase
    {
        private string _name = null!;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
    }
}
