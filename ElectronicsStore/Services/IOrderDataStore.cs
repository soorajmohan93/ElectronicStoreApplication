using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElectronicsStore.Models;

namespace ElectronicsStore.Services
{
    public interface IOrderDataStore<T>
    {
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<Order> GetOrderAsync(int orderId);
        Task AddOrderAsync(Order order);
        Task DeleteOrderAsync(Order order);
    }
}
