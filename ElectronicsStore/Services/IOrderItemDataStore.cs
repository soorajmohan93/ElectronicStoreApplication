using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElectronicsStore.Models;

namespace ElectronicsStore.Services
{
    public interface IOrderItemDataStore<T>
    {
        Task<IEnumerable<OrderItem>> GetOrderItemsAsync(int orderId);
        Task<Order> GetOrderItemAsync(int orderItemId);
        Task AddOrderItemsAsync(List<OrderItem> orderItems);
        Task DeleteOrderItemAsync(OrderItem orderItem);
    }
}
