using AppAutoUpdate.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AppAutoUpdate.Services
{
    public class FileService : IFileService
    {
        public bool Exists(string route)
        {
            try
            {
                return File.Exists(route);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public string Read(string route)
        {
            try
            {
                if (Exists(route))
                    return File.ReadAllText(route).ToString();

                return "File not exists";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }

        public bool Remove(string route)
        {
            try
            {
                File.Delete(route);
                Console.WriteLine($"The file {route} has been deleted!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}