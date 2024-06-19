using System.Threading.Tasks;
using Xamarin.Forms;

namespace ToDoList.Services.MessageDialog
{
    public class MessageDialogService : IMessageDialogService
    {
        private readonly Page _page;

        public MessageDialogService(Page page)
        {
            _page = page;
        }
        public void ShowMessage(string title, string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                _page.DisplayAlert(title, message, "OK");
            });
        }

        public async System.Threading.Tasks.Task ShowMessageAsync(string title, string message)
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                await _page.DisplayAlert(title, message, "OK");
            });
        }

        public async Task<bool> ShowMessageConfirmAsync(string title, string message)
        {
            return await Shell.Current.DisplayAlert(title, message, "Tak", "Nie");
        }
    }
}
