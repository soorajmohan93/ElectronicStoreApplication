using System;
using System.Threading.Tasks;

namespace ElectronicsStore.Services
{
    public interface IMessageService
    {
        void ShortAlert(string message);
        void LongAlert(string message);
        Task<bool> ShowAsync(string question, string message, string option1, string option2);
    }
}
