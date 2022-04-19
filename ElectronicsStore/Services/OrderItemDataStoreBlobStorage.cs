using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using ElectronicsStore.Models;
using Newtonsoft.Json;

namespace ElectronicsStore.Services
{
    public class OrderItemDataStoreBlobStorageJson : IOrderItemDataStore<OrderItem>
    {
        private readonly BlobServiceClient service = new BlobServiceClient(ConnectionString);

        private static string ConnectionString => Constants.ConnectionString;

        private static string Container => "data";

        private static string FileName => "OrderItems.json";

        public async Task WriteFile(List<OrderItem> orderItems)
        {
            var jsonString = JsonConvert.SerializeObject(orderItems);

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(jsonString);
            writer.Flush();
            stream.Position = 0;

            BlobClient blob = service.GetBlobContainerClient(Container).GetBlobClient(FileName);
            await blob.UploadAsync(stream, overwrite: true);
        }

        public async Task<List<OrderItem>> ReadFile()
        {
            BlobClient blob = service.GetBlobContainerClient(Container).GetBlobClient(FileName);

            if (blob.Exists())
            {
                var stream = new MemoryStream();
                await blob.DownloadToAsync(stream);

                stream.Position = 0;

                var jsonString = new StreamReader(stream).ReadToEnd();

                var orderItems = JsonConvert.DeserializeObject<List<OrderItem>>(jsonString);

                return orderItems;
            }

            else
            {
                var defaultOrderItems = new List<OrderItem>();
                await WriteFile(defaultOrderItems);

                return defaultOrderItems;
            }
        }

        public async Task AddOrderItemsAsync(List<OrderItem> orderItems)
        {
            var orders = await ReadFile();
            orders.AddRange(orderItems);

            await WriteFile(orders);
        }

        public async Task DeleteOrderItemAsync(OrderItem orderItem)
        {
            var orderItems = await ReadFile();
            var remove = orderItems.Find(p => p.OrderItemId == orderItem.OrderItemId);
            orderItems.Remove(remove);
            await WriteFile(orderItems);
        }

        public Task<Order> GetOrderItemAsync(int orderItemId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsAsync(int orderId)
        {
            var AllOrderItems = await ReadFile();
            var OrderItems = new List<OrderItem>();

            foreach(var orderItem in AllOrderItems)
            {
                if (orderItem.Order.OrderId == orderId)
                    OrderItems.Add(orderItem);
            }

            return OrderItems;
        }
    }
}
