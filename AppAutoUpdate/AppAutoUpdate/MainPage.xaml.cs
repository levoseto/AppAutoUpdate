using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using AppAutoUpdate.Interfaces;
using System.IO;

namespace AppAutoUpdate
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            spanVersion.Text = VersionTracking.CurrentVersion;
        }

        private void ObtieneAPKDisponible()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var servicioRutas = DependencyService.Get<IRutas>();
                var rutaEscritura = servicioRutas.GetApkRoute();
                var nombreApk = servicioRutas.GetApkName();
                if (!string.IsNullOrEmpty(rutaEscritura))
                {
                    var rutaCompletaApk = Path.Combine(rutaEscritura, $"{nombreApk}.apk");
                    if (File.Exists(rutaCompletaApk))
                    {
                        var servicioApk = DependencyService.Get<IFileOpener>();
                        servicioApk.OpenApk(rutaCompletaApk);
                    }
                    else
                    {
                        App.Current.MainPage.DisplayAlert("Hola", "APK no existe", "Aceptar");
                    }
                }
            });
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            ObtieneAPKDisponible();
        }
    }
}