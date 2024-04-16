﻿using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Dtos;
using Xamarin.Forms;

namespace ToDoList.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<bool> LoginAsync(LoginDto dto)
        {
            
            var stringContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var httpClient = _httpClientFactory.CreateClient("httpClient");
            using (var response = await httpClient.PostAsync("Account/Login", stringContent))
            {
                if(!(response.StatusCode == HttpStatusCode.OK))
                    return false;

                var accessToken = await response.Content.ReadAsStringAsync();

                await Xamarin.Essentials.SecureStorage.SetAsync("AccessToken", accessToken);

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                return true;
            }
        }

        public async Task<bool> RegisterAsync(RegisterUserDto dto)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var httpClient = _httpClientFactory.CreateClient("httpClient");

            using (var response = await httpClient.PostAsync("Account/Register", stringContent))
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
