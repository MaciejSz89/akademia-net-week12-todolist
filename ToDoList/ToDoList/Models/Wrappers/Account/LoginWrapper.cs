namespace ToDoList.Models.Wrappers.Account
{
    public class LoginWrapper : WrapperBase
    {
        private string _email = null!;
        private string _password = null!;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

    }
}
