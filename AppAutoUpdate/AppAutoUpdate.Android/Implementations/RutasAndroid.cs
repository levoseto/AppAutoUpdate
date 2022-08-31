using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AppAutoUpdate.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(AppAutoUpdate.Droid.Implementations.RutasAndroid))]

namespace AppAutoUpdate.Droid.Implementations
{
    public class RutasAndroid : IRutas
    {
        public string GetApkName()
        {
            return Platform.CurrentActivity.ApplicationContext.PackageName;
        }

        public string GetApkRoute()
        {
            return Application.Context.GetExternalFilesDir(string.Empty).ToString();
        }
    }
}