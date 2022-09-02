using AppAutoUpdate.Interfaces;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppAutoUpdate.Services
{
    public class FtpAppConnectionService : IFtpAppConnectionService
    {
        private static readonly string _DireccionFTP = "201.144.21.119";
        private static IRutas _Rutas = DependencyService.Get<IRutas>();

        public async Task GetApk(string route)
        {
            try
            {
                var rutaEscritura = _Rutas.GetApkRoute();

                using (var client = new Rebex.Net.Ftp())
                {
                    client.TransferProgressChanged += Client_TransferProgressChanged;
                    // connect and log in
                    await client.ConnectAsync(_DireccionFTP);
                    await client.LoginAsync("teklogix", "t3kLog1xmeX");

                    await client.DownloadAsync(route, rutaEscritura, Rebex.IO.TraversalMode.Recursive, Rebex.IO.TransferMethod.Copy, Rebex.IO.ActionOnExistingFiles.OverwriteAll);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        private void Client_TransferProgressChanged(object sender, Rebex.Net.FtpTransferProgressChangedEventArgs e)
        {
            var progreso = e.ProgressPercentage;
            Console.WriteLine(progreso);
            if (progreso.Equals("100"))
            {
            }
        }

        public bool IsConnected()
        {
            using (var client = new Rebex.Net.Ftp())
            {
                // connect and log in
                client.Connect(_DireccionFTP);
                client.Login("teklogix", "t3kLog1xmeX");
                return client.IsConnected;
            }
        }
    }
}