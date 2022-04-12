using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElectronicsStore.Models;
using ElectronicsStore.Services;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Xamarin.Forms;

namespace ElectronicsStore.ViewModels
{
    [QueryProperty(nameof(OrderId), nameof(OrderId))]
    public class OrderItemViewModel: ElectronicStoreManagerBase
    {
        public ObservableRangeCollection<OrderItem> OrderItems { get; set; }

        public AsyncCommand RefreshCommand { get; }

        public AsyncCommand TrackingCommand { get; }
        public AsyncCommand CancelOrderCommand { get; }

        private string resultText;

        public string ResultText
        {
            get => resultText;
            set => SetProperty(ref resultText, value);
        }

        private int orderId;
        public int OrderId
        {
            get
            {
                return orderId;
            }
            set
            {
                orderId = value;
                ResultText = "Showing Order Number " + value;
                OrderItems.Clear();
                LoadOrderItems(value);
            }
        }

        public OrderItemViewModel()
        {

            OrderItems = new ObservableRangeCollection<OrderItem>();
            OrderItems.Clear();
            LoadOrderItems(orderId);

            RefreshCommand = new AsyncCommand(Refresh);
            TrackingCommand = new AsyncCommand(Track);
            CancelOrderCommand = new AsyncCommand(Cancel);
        }


        public async Task Refresh()
        {
            IsBusy = true;

            OrderItems.Clear();
            LoadOrderItems(orderId);

            IsBusy = false;
        }

        public async void LoadOrderItems(int id)
        {
            IEnumerable<OrderItem> orderItems = await OrderItemDataStore.GetOrderItemsAsync(id);
            OrderItems.AddRange(orderItems);
        }

        public async Task Track()
        {
            var route = $"{nameof(Views.OrderTrackingPage)}?OrderId={OrderId}";
            await Shell.Current.GoToAsync(route);
        }

        public async Task Cancel()
        {
            var result = await DependencyService.Get<IMessageService>().ShowAsync("Confirm", $"Do you want to Cancel Order {orderId}", "Yes", "No");

            if (result)
            {
                Order order = await OrderDataStore.GetOrderAsync(orderId);
                await OrderDataStore.DeleteOrderAsync(order);

                foreach(var orderItem in OrderItems)
                {
                    await OrderItemDataStore.DeleteOrderItemAsync(orderItem);
                }

                IEnumerable<OrderTracking> orderTrackings = await OrderTrackingDataStore.GetOrderTrackingAsync(orderId);
                foreach(var tracking in orderTrackings)
                {
                    await OrderTrackingDataStore.DeleteOrderTrackingAsync(tracking);
                }

                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
