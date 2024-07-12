using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Dtos;

namespace ToDoList.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;

        public AccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> LoginAsync(LoginDto dto)
        {
            
            var stringContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            using (var response = await _httpClient.PostAsync("Account/Login", stringContent))
            {
                if(!(response.StatusCode == HttpStatusCode.OK))
                    return false;

                await Xamarin.Essentials.SecureStorage.SetAsync("Email", dto.Email);
                await Xamarin.Essentials.SecureStorage.SetAsync("Password", dto.Password);

                var accessToken = await response.Content.ReadAsStringAsync();

                await Xamarin.Essentials.SecureStorage.SetAsync("AccessToken", accessToken);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                return true;
            }
        }

        public async Task<bool> RegisterAsync(RegisterUserDto dto)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            using (var response = await _httpClient.PostAsync("Account/Register", stringContent))
            {
                var isSuccess = response.IsSuccessStatusCode;

                if(isSuccess)
                {
                    await Xamarin.Essentials.SecureStorage.SetAsync("Email", dto.Email);
                    await Xamarin.Essentials.SecureStorage.SetAsync("Password", dto.Password);
                }

                return isSuccess;
            }
        }
    }
}
