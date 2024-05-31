using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using ToDoList;

// PlatformHttpClientHandler.cs w projekcie Android
[assembly: Xamarin.Forms.Dependency(typeof(ToDoList.Droid.PlatformHttpMessageHandler))]
namespace ToDoList.Droid
{
    public class PlatformHttpMessageHandler : ICustomHttpMessageHandler
    {
        public HttpMessageHandler GetHttpMessageHandler()
        {
            return new Xamarin.Android.Net.AndroidClientHandler()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
        }
    }
}
