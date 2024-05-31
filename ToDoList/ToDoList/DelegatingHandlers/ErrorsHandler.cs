using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ToDoList.Services;
using Xamarin.Forms;

namespace ToDoList.DelegatingHandlers
{
    public class ErrorsHandler : DelegatingHandler
    {
        private readonly IMessageDialog _messageDialog;

        public ErrorsHandler(IMessageDialog messageDialog)
        {
            _messageDialog = messageDialog;
        }


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                     CancellationToken cancellationToken)
        {

            try
            {
                var response = await base.SendAsync(request, cancellationToken);

                await HandleErrors(response, request.RequestUri.LocalPath.ToLower());

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await Shell.Current.GoToAsync("//LoginPage");
                }

                return response;


            }
            catch (HttpRequestException)
            {
                await ShowError("Błąd żądania");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                await ShowError("Błąd");
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }


        private async Task HandleErrors(HttpResponseMessage response, string uriLocalPath)
        {
            string errorTitle = "Błąd";
            string errorMessage;
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    errorMessage = "Błąd autoryzacji";
                    break;
                case HttpStatusCode.Forbidden:
                    errorMessage = "Niedozwolona operacja";
                    break;
                case HttpStatusCode.BadRequest:
                    errorMessage = uriLocalPath.Contains("/account/login") ? "Błędny login lub hasło" : "Błąd żądania";
                    break;
                case HttpStatusCode.NotFound:
                    if (uriLocalPath.Contains("task"))
                        errorMessage = "Nie znaleziono zadania";
                    else if (uriLocalPath.Contains("category"))
                        errorMessage = "Nie znaleziono kategorii";
                    else
                        errorMessage = "Nie znaleziono zasobu";
                    break;
                case HttpStatusCode.InternalServerError:
                    errorMessage = "Błąd serwera";
                    break;
                default:
                    errorMessage = null;
                    break;
            }

            if (errorMessage != null)
                await ShowError(errorMessage, errorTitle);
        }

        private async Task ShowError(string message, string title = "Błąd")
        {
            await _messageDialog.ShowMessageAsync(title, message);
        }

    }
}
