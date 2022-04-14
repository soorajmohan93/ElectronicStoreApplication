using System;
using System.IO;
using System.Threading.Tasks;

namespace ElectronicsStore.Services
{
    public interface IBlobStorageService
    {
        Task UploadStreamAsync(string name, MemoryStream memory);
        Task<MemoryStream> DownloadStreamAsync(string name);
    }
}
