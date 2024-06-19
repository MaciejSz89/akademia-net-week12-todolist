namespace ToDoList.Models.Wrappers.Category
{
    public class ReadCategoryWrapper : WrapperBase
    {
        private int _id;
        private string _name = null!;

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
    }
}
