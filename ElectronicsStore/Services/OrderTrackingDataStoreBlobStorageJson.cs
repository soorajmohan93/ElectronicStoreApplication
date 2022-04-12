using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using ElectronicsStore.Models;
using Newtonsoft.Json;

namespace ElectronicsStore.Services
{
    public class OrderTrackingDataStoreBlobStorageJson : IOrderTrackingDataStore<OrderTracking>
    {
        private readonly BlobServiceClient service = new BlobServiceClient(ConnectionString);

        private static string ConnectionString => "DefaultEndpointsProtocol=https;AccountName=soorajjobmanager;AccountKey=yyCKVbnBK7pMN9hzKyqSK1sRwwELDoqXSPDxjPLTwOxGvccqysrEigv2hSGqrKiCyLuxtTEsB2TL+ASthcQWOA==;EndpointSuffix=core.windows.net";

        private static string Container => "data";

        private static string FileName => "OrderTracking.json";

        public async Task WriteFile(List<OrderTracking> trackingItems)
        {
            var jsonString = JsonConvert.SerializeObject(trackingItems);

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(jsonString);
            writer.Flush();
            stream.Position = 0;

            BlobClient blob = service.GetBlobContainerClient(Container).GetBlobClient(FileName);
            await blob.UploadAsync(stream, overwrite: true);
        }

        public async Task<List<OrderTracking>> ReadFile()
        {
            BlobClient blob = service.GetBlobContainerClient(Container).GetBlobClient(FileName);

            if (blob.Exists())
            {
                var stream = new MemoryStream();
                await blob.DownloadToAsync(stream);

                stream.Position = 0;

                var jsonString = new StreamReader(stream).ReadToEnd();

                var orderItems = JsonConvert.DeserializeObject<List<OrderTracking>>(jsonString);

                return orderItems;
            }

            else
            {
                var defaultTrackingItems = new List<OrderTracking>();
                await WriteFile(defaultTrackingItems);

                return defaultTrackingItems;
            }
        }

        public async Task AddOrderTrackingAsync(OrderTracking trackingItem)
        {
            var tracking = await ReadFile();
            tracking.Add(trackingItem);

            await WriteFile(tracking);
        }

        public async Task DeleteOrderTrackingAsync(OrderTracking trackingItem)
        {
            var trackings = await ReadFile();
            var remove = trackings.Find(p => p.OrderTrackingTimeStamp == trackingItem.OrderTrackingTimeStamp);
            trackings.Remove(remove);
            await WriteFile(trackings);
        }

        public async Task<IEnumerable<OrderTracking>> GetOrderTrackingAsync(int orderId)
        {
            var allOrderTracking = await ReadFile();
            var orderTrackingItems = new List<OrderTracking>();

            foreach (var orderItem in allOrderTracking)
            {
                if (orderItem.Order.OrderId == orderId)
                    orderTrackingItems.Add(orderItem);
            }

            return orderTrackingItems;
        }
    }
}
