using System;
using ElectronicsStore.Models;
using ElectronicsStore.Services;
using MvvmHelpers;
using Xamarin.Forms;

namespace ElectronicsStore.ViewModels
{
    public class ElectronicStoreManagerBase: BaseViewModel
    {
        public IProductDataStore<Product> ProductDataStore => DependencyService.Get<IProductDataStore<Product>>();
        public ICartDataStore<Cart> CartDataStore => DependencyService.Get<ICartDataStore<Cart>>();
        public IOrderDataStore<Order> OrderDataStore => DependencyService.Get<IOrderDataStore<Order>>();
        public IOrderItemDataStore<OrderItem> OrderItemDataStore => DependencyService.Get<IOrderItemDataStore<OrderItem>>();
        public IOrderTrackingDataStore<OrderTracking> OrderTrackingDataStore => DependencyService.Get<IOrderTrackingDataStore<OrderTracking>>();
    }
}
