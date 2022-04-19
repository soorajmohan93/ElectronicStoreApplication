using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using ElectronicsStore.Models;
using Newtonsoft.Json;

namespace ElectronicsStore.Services
{
    public class OrderDataStoreBlobStorageJson: IOrderDataStore<Order>
    {
        private readonly BlobServiceClient service = new BlobServiceClient(ConnectionString);

        private static string ConnectionString => Constants.ConnectionString;

        private static string Container => "data";

        private static string FileName => "Orders.json";

        public async Task WriteFile(List<Order> orders)
        {
            var jsonString = JsonConvert.SerializeObject(orders);

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(jsonString);
            writer.Flush();
            stream.Position = 0;

            BlobClient blob = service.GetBlobContainerClient(Container).GetBlobClient(FileName);
            await blob.UploadAsync(stream, overwrite: true);
        }

        public async Task<List<Order>> ReadFile()
        {
            BlobClient blob = service.GetBlobContainerClient(Container).GetBlobClient(FileName);

            if (blob.Exists())
            {
                var stream = new MemoryStream();
                await blob.DownloadToAsync(stream);

                stream.Position = 0;

                var jsonString = new StreamReader(stream).ReadToEnd();

                var orders = JsonConvert.DeserializeObject<List<Order>>(jsonString);

                return orders;
            }

            else
            {
                var defaultOrders = new List<Order>();
                await WriteFile(defaultOrders);

                return defaultOrders;
            }
        }

        public async Task AddOrderAsync(Order order)
        {
            var orders = await ReadFile();
            if (orders == null)
            {
                orders = new List<Order>() { new Order()
                { OrderId = order.OrderId, OrderValue = order.OrderValue } };
            }
            else
            {
                orders.Add(order);
            }

            await WriteFile(orders);
        }

        public async Task DeleteOrderAsync(Order order)
        {
            var orders = await ReadFile();
            var remove = orders.Find(p => p.OrderId == order.OrderId);
            orders.Remove(remove);
            await WriteFile(orders);
        }

        public async Task<Order> GetOrderAsync(int orderId)
        {
            var orders = await ReadFile();
            var order = orders.Find(p => p.OrderId == orderId);
            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await ReadFile();
        }
    }
}
