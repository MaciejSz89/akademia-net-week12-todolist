using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Core.Dtos;
using ToDoList.Models.Converters;
using ToDoList.Models.Wrappers.Account;
using ToDoList.Services.Account;
using ToDoList.Views;
using ToDoList.Views.Account;
using ToDoList.Views.Task;
using Xamarin.Forms;

namespace ToDoList.ViewModels.Account
{
    public class LoginViewModel : BaseViewModel, ILoginViewModel
    {
        private readonly IAccountService _accountService;
        private LoginWrapper _login;

        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }

        public LoginWrapper Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        public LoginViewModel()
        {
            Login = new LoginWrapper();

            try
            {
                var email = Xamarin.Essentials.SecureStorage.GetAsync("Email").Result;
                var password = Xamarin.Essentials.SecureStorage.GetAsync("Password").Result;

                if (!string.IsNullOrEmpty(email)
                    && !string.IsNullOrEmpty(password))
                {
                    Login.Email = email;
                    Login.Password = password;
                }
            }
            finally
            {

            }


            LoginCommand = new Command(OnLoginClicked);
            RegisterCommand = new Command(OnRegisterClicked);
            _accountService = Startup.ServiceProvider.GetService<IAccountService>();
        }

        private async void OnLoginClicked(object obj)
        {
            IsBusy = true;

            var loginUserDto = (obj as LoginWrapper).ToDto();
            var isSuccess = await _accountService.LoginAsync(loginUserDto);
            if (isSuccess)
            {
                await Shell.Current.DisplayAlert("Zalogowano pomyślnie", $"Użytkownik {loginUserDto.Email} zalogował się pomyślnie", "OK");
                await Shell.Current.GoToAsync($"//{nameof(TasksPage)}");
            }
            else
            {
                await Shell.Current.DisplayAlert("Błąd logowania", $"Spróbuj ponownie później.", "OK");
            }
            IsBusy = false;
        }

        private async void OnRegisterClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
        }
    }
}
