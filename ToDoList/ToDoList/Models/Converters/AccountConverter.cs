using ToDoList.Core.Dtos;
using ToDoList.Models.Wrappers.Account;

namespace ToDoList.Models.Converters
{
    public static class AccountConverter
    {
        public static LoginDto ToDto(this LoginWrapper wrapper)
        {
            var loginDto = new LoginDto
            {
                Email = wrapper.Email,
                Password = wrapper.Password
            };
            return loginDto;
        }

        public static RegisterUserDto ToDto(this RegisterWrapper wrapper)
        {
            var registerDto = new RegisterUserDto
            {
                Email = wrapper.Email,
                Password = wrapper.Password,
                ConfirmPassword = wrapper.ConfirmPassword,
                FirstName = wrapper.FirstName,
                LastName = wrapper.LastName,
            };
            return registerDto;
        }
    }
}
