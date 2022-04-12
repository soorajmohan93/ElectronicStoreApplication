using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElectronicsStore.Models;

namespace ElectronicsStore.Services
{
    public interface IOrderTrackingDataStore<T>
    {
        Task<IEnumerable<OrderTracking>> GetOrderTrackingAsync(int orderId);
        Task AddOrderTrackingAsync(OrderTracking trackingItem);
        Task DeleteOrderTrackingAsync(OrderTracking trackingItem);
    }
}
