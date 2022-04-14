using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace ElectronicsStore.Services
{
    public class BlobAzureStorage: IBlobStorageService
    {

        private readonly BlobServiceClient service = new BlobServiceClient(ConnectionString);

        private static string ConnectionString => "DefaultEndpointsProtocol=https;AccountName=soorajjobmanager;AccountKey=yyCKVbnBK7pMN9hzKyqSK1sRwwELDoqXSPDxjPLTwOxGvccqysrEigv2hSGqrKiCyLuxtTEsB2TL+ASthcQWOA==;EndpointSuffix=core.windows.net";

        private static string Container => "data";


        public async Task<MemoryStream> DownloadStreamAsync(string name)
        {
            BlobClient blob = service.GetBlobContainerClient(Container).GetBlobClient(name);

            if (blob.Exists())
            {
                var stream = new MemoryStream();
                await blob.DownloadToAsync(stream);

                stream.Position = 0;

                return stream;
            }

            return null;
        }

        public async Task UploadStreamAsync(string name, MemoryStream memory)
        {
            try
            {
                memory.Position = 0;

                BlobClient blob = service.GetBlobContainerClient(Container).GetBlobClient(name);

                await blob.UploadAsync(memory);

            }
            catch
            {

            }

        }
    }
}
