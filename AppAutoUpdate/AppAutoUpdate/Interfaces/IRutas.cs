using System;
using System.Collections.Generic;
using System.Text;

namespace AppAutoUpdate.Interfaces
{
    public interface IRutas
    {
        string GetApkDataFolderRoute();

        string GetApkName();
    }
}