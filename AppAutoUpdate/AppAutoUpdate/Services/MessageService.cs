using System;
using System.Threading.Tasks;

using Xamarin.Essentials;

namespace AppAutoUpdate.Services
{
    public static class MessageService
    {
        public static async Task Show(string message)
        {
            try
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await App.Current.MainPage.DisplayAlert("AppAutoUpdate", message, "Aceptar");
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}