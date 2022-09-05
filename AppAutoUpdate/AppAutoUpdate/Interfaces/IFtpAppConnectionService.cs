using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppAutoUpdate.Interfaces
{
    public interface IFtpAppConnectionService
    {
        bool IsConnected();

        Task GetFileAsync(string route);
    }
}