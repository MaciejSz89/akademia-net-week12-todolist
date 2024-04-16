using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Dtos;

namespace ToDoList.Services.Account
{
    public interface IAccountService
    {
        Task<bool> RegisterAsync(RegisterUserDto dto);
        Task<bool> LoginAsync(LoginDto dto);
    }
}
