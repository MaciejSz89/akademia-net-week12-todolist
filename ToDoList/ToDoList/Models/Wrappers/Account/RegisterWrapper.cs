namespace ToDoList.Models.Wrappers.Account
{
    public class RegisterWrapper : WrapperBase
    {
        private string _email = null!;
        private string _password = null!;
        private string _confirmPassword = null!;
        private string _firstName = null!;
        private string _lastName = null!;

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
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }
    }
}
