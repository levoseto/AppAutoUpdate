using AppAutoUpdate.Interfaces;
using AppAutoUpdate.Services;

using AsyncAwaitBestPractices;

using System;
using System.IO;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppAutoUpdate
{
    public partial class MainPage : ContentPage
    {
        private static readonly IFileService _FileService = new FileService();
        private static readonly IRutas _RutasService = DependencyService.Get<IRutas>();
        private static readonly IFtpAppConnectionService _FtpConnectionService = new FtpAppConnectionService();

        private static readonly string _RutaEscrituraArchivosApp = _RutasService.GetApkDataFolderRoute();
        private static readonly string _NombreApk = _RutasService.GetApkName();
        private static readonly string _Version = VersionTracking.CurrentVersion;
        private static readonly string _RutaUbicacionFTP = "/Xamarin.SCAPMobile";
        private static readonly string _NombreArchivoVersion = "AutoUpdateVersion.txt";

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            spanVersion.Text = _Version;
            slContenido.IsVisible = true;
            slLoading.IsVisible = false;

            GetInfoVersionFile().SafeFireAndForget();
        }

        private void ObtieneAPKDisponible()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var servicioRutas = DependencyService.Get<IRutas>();
                var rutaEscritura = servicioRutas.GetApkDataFolderRoute();
                var nombreApk = servicioRutas.GetApkName();

                if (!string.IsNullOrEmpty(rutaEscritura))
                {
                    var rutaCompletaApk = Path.Combine(rutaEscritura, $"{nombreApk}.apk");
                    if (_FileService.Exists(rutaCompletaApk))
                    {
                        var servicioApk = DependencyService.Get<IFileOpener>();
                        servicioApk.OpenApk(rutaCompletaApk);
                    }
                    else
                    {
                        await MessageService.Show("APK no existe");
                    }
                }
            });
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            ObtieneAPKDisponible();
        }

        private async void ftpButton_Clicked(object sender, EventArgs e)
        {
            IFtpAppConnectionService connectionService = new FtpAppConnectionService();
            if (connectionService != null)
            {
                var resultadoConexion = connectionService.IsConnected();
                if (resultadoConexion)
                {
                    slContenido.IsVisible = false;
                    slLoading.IsVisible = true;
                    await connectionService.GetFileAsync("/Xamarin.SCAPMobile/com.levoapps.appautoupdate.apk");
                    ObtieneAPKDisponible();
                    return;
                }

                await DisplayAlert("AppAutoUpdate", "No hay conexión al FTP", "Aceptar");
            }
        }

        private async Task GetInfoVersionFile()
        {
            try
            {
                var rutaFtpArchivoVersion = $"{_RutaUbicacionFTP}/{_NombreArchivoVersion}";
                var rutaLocalArchivoVersion = $"{_RutaEscrituraArchivosApp}/{_NombreArchivoVersion}";
                var rutaLocalApk = $"{_RutaEscrituraArchivosApp}/{_NombreApk}";

                if (_FtpConnectionService.IsConnected())
                {
                    await _FtpConnectionService.GetFileAsync(rutaFtpArchivoVersion);

                    // Leer archivo de la versión
                    if (_FileService.Exists(rutaLocalArchivoVersion))
                    {
                        var versionObtenidaArchivoLocal = _FileService.Read(rutaLocalArchivoVersion);
                        if (!versionObtenidaArchivoLocal.Equals(_Version))
                        {
                            var versionFloat = float.Parse(_Version);
                            var versionObtenidaArchivoLocalFloat = float.Parse(versionObtenidaArchivoLocal);

                            if (versionObtenidaArchivoLocalFloat > versionFloat)
                            {
                                await MessageService.Show("Hay una nueva versión disponible");
                                return;
                            }
                        }

                        // TODO: Si la versión es igual, siempre eliminar el archivo de versión y el APK si existe descargado.
                        _FileService.Remove(rutaLocalArchivoVersion);
                        _FileService.Remove(rutaLocalApk);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}