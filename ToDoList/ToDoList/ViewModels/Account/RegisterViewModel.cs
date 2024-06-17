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
using Xamarin.Forms;

namespace ToDoList.ViewModels.Account
{
    public class RegisterViewModel : ViewModelBase, IRegisterViewModel
    {
        private readonly IAccountService _accountService;
        private RegisterWrapper _register = null!;

        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }
        public RegisterWrapper RegisterUser
        {
            get => _register;
            set
            {
                _register = value;
                OnPropertyChanged();
            }
        }

        public string Message { get; set; } = "";

        public RegisterViewModel()
        {
            RegisterUser = new RegisterWrapper();
            LoginCommand = new Command(OnLoginClicked);
            RegisterCommand = new Command(OnRegisterClicked);
            _accountService = Startup.ServiceProvider.GetService<IAccountService>()!;
        }

        private async void OnLoginClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        private async void OnRegisterClicked(object obj)
        {
            IsBusy = true;
            var registerUserDto = (obj as RegisterWrapper)?.ToDto();
            if (registerUserDto == null) 
                return;

            var isSuccess = await _accountService.RegisterAsync(registerUserDto);
            if (isSuccess)
            {
                await Shell.Current.DisplayAlert("Rejestracja przebiegła pomyślnie", $"Użytkownik {registerUserDto.Email} został pomyślnie zarejestrowany", "OK");
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            else
            {
                await Shell.Current.DisplayAlert("Błąd rejestracji", $"Spróbuj ponownie później.", "OK");
            }
            IsBusy = false;
        }
    }
}
