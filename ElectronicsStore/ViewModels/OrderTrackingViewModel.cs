using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using ElectronicsStore.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Xamarin.Forms;

namespace ElectronicsStore.ViewModels
{
    [QueryProperty(nameof(OrderId), nameof(OrderId))]
    public class OrderTrackingViewModel: ElectronicStoreManagerBase
    {
        public ObservableRangeCollection<OrderTracking> OrderTrackingItems { get; set; }

        public AsyncCommand RefreshCommand { get; }

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
                ResultText = "Tracking for Order Number: " + value;
                OrderTrackingItems.Clear();
                LoadOrderTracking(value);
            }
        }

        public OrderTrackingViewModel()
        {
            OrderTrackingItems = new ObservableRangeCollection<OrderTracking>();
            OrderTrackingItems.Clear();
            LoadOrderTracking(orderId);

            RefreshCommand = new AsyncCommand(Refresh);
        }

        public async Task Refresh()
        {
            IsBusy = true;

            OrderTrackingItems.Clear();
            LoadOrderTracking(orderId);

            IsBusy = false;
        }

        public async void LoadOrderTracking(int Id)
        {

            IEnumerable<OrderTracking> orderTracking = await OrderTrackingDataStore.GetOrderTrackingAsync(Id);
            await UpdateStatusAsync(orderTracking: orderTracking);
            OrderTrackingItems.AddRange(orderTracking);
        }

        public async Task UpdateStatusAsync (IEnumerable<OrderTracking> orderTracking)
        {
            string newStatus = null;

            foreach (var track in orderTracking){
                switch(track.OrderTrackingStatus)
                {
                    case "Order Placed":
                        newStatus = "Order is Being Processed";
                        break;
                    case "Order is Being Processed":
                        newStatus = "Order is Packed";
                        break;
                    case "Order is Packed":
                        newStatus = "Order is on Transit";
                        break;
                    case "Order is on Transit":
                        newStatus = "Order is Delivered";
                        break;
                    case "Order is Delivered":
                        newStatus = null;
                        break;
                    default:
                        newStatus = null;
                        break;
                }
            }

            if (newStatus != null)
            {
                Order order = await OrderDataStore.GetOrderAsync(orderId);
                DateTime currentTimestamp = DateTime.Now;
                var culture = new CultureInfo("en-US");

                OrderTracking newOrderTracking = new OrderTracking() { Order = order, OrderTrackingStatus = newStatus, OrderTrackingTimeStamp = currentTimestamp.ToString(culture) };
                await OrderTrackingDataStore.AddOrderTrackingAsync(newOrderTracking);
            }
        }
    }
}
