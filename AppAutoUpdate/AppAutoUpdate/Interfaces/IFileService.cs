using System;
using System.Collections.Generic;
using System.Text;

namespace AppAutoUpdate.Interfaces
{
    public interface IFileService
    {
        bool Remove(string route);

        bool Exists(string route);

        string Read(string route);
    }
}