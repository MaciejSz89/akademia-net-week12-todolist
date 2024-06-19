using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ToDoList.DelegatingHandlers
{
    public class GetCachedAccessTokenHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, 
                                                               CancellationToken cancellationToken)
        {
            var accessToken = await SecureStorage.GetAsync("AccessToken");


            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }


            return await base.SendAsync(request, cancellationToken);
        }
    }
}
