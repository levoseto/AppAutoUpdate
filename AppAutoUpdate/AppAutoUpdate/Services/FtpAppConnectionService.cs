using AppAutoUpdate.Interfaces;
using System;
using System.Threading.Tasks;

namespace AppAutoUpdate.Services
{
    public class FtpAppConnectionService : IFtpAppConnectionService
    {
        public async Task GetApk(string route)
        {
            using (var client = new Rebex.Net.Ftp())
            {
                // connect and log in
                await client.ConnectAsync("172.25.2.108");
                await client.LoginAsync("teklogix", "t3kLog1xmeX");
            }
        }

        public bool IsConnected()
        {
            using (var client = new Rebex.Net.Ftp())
            {
                // connect and log in
                client.Connect("172.25.2.108");
                client.Login("teklogix", "t3kLog1xmeX");
                return client.IsConnected;
            }
        }
    }
}