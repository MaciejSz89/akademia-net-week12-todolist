using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(ToDoList.iOS.PlatformHttpMessageHandler))]
namespace ToDoList.iOS
{
    public class PlatformHttpMessageHandler : ICustomHttpMessageHandler
    {
        public HttpMessageHandler GetHttpMessageHandler()
        {
            return new NSUrlSessionHandler();
        }
    }
}