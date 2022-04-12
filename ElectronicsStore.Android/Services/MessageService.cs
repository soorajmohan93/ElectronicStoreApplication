using System;
using System.Threading.Tasks;
using Android.App;
using Android.Widget;
using ElectronicsStore.Droid.Services;
using ElectronicsStore.Services;

[assembly: Xamarin.Forms.Dependency(typeof(MessageService))]
namespace ElectronicsStore.Droid.Services
{
    public class MessageService : IMessageService
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }

        public async Task<bool> ShowAsync(string question, string message, string option1, string option2)
        {
            bool result = await App.Current.MainPage.DisplayAlert(question, message, option1, option2);
            return result;
        }
    }
}
