using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ToDoList.Services
{
    public class MessageDialog : IMessageDialog
    {
        private readonly Page _page;

        public MessageDialog(Page page)
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
    }
}
