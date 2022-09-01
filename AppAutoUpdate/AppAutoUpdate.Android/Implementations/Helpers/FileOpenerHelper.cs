using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using AppAutoUpdate.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(AppAutoUpdate.Droid.Implementations.Helpers.FileOpenerHelper))]

namespace AppAutoUpdate.Droid.Implementations.Helpers
{
    public class FileOpenerHelper : IFileOpener
    {
        public void OpenApk(string filepath)
        {
            var file = new Java.IO.File(filepath);
            Intent install = new Intent(Intent.ActionView);

            // Old Approach
            if (Android.OS.Build.VERSION.SdkInt < BuildVersionCodes.N)
            {
                install.SetFlags(ActivityFlags.NewTask | ActivityFlags.GrantReadUriPermission);
                install.SetDataAndType(Android.Net.Uri.FromFile(file), "application/vnd.android.package-archive"); //mimeType
            }
            else
            {
                Android.Net.Uri apkURI = AndroidX.Core.Content.FileProvider.GetUriForFile(Platform.CurrentActivity.BaseContext, Platform.CurrentActivity.BaseContext.PackageName + ".provider", file);
                install.SetDataAndType(apkURI, "application/vnd.android.package-archive");
                install.AddFlags(ActivityFlags.NewTask);
                install.AddFlags(ActivityFlags.GrantReadUriPermission);
            }

            Platform.OnNewIntent(install);
            Platform.CurrentActivity.StartActivity(install);
        }
    }
}