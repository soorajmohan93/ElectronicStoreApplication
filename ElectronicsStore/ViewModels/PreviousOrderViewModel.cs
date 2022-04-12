using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElectronicsStore.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Xamarin.Forms;

namespace ElectronicsStore.ViewModels
{
    public class PreviousOrderViewModel : ElectronicStoreManagerBase
    {
        public ObservableRangeCollection<Order> Orders { get; set; }

        public AsyncCommand RefreshCommand { get; }

        public AsyncCommand PageAppearingCommand { get; }
        public AsyncCommand PageDisappearingCommand { get; }

        public AsyncCommand<Order> TappedCommand { get; }

        public PreviousOrderViewModel()
        {
            Orders = new ObservableRangeCollection<Order>();


            RefreshCommand = new AsyncCommand(Refresh);

            PageAppearingCommand = new AsyncCommand(PageAppearing);
            PageDisappearingCommand = new AsyncCommand(PageDisappearing);

            TappedCommand = new AsyncCommand<Order>(OpenOrder);
        }


        async Task PageAppearing()
        {
            Orders.Clear();
            LoadOrders();
        }

        async Task PageDisappearing()
        {
        }


        public async Task Refresh()
        {
            IsBusy = true;

            Orders.Clear();
            LoadOrders();

            IsBusy = false;
        }

        public async void LoadOrders()
        {
            IEnumerable<Order> orders = await OrderDataStore.GetOrdersAsync();
            Orders.AddRange(orders);
        }

        public async Task OpenOrder(Order order)
        {
            var route = $"{nameof(Views.OrderItemPage)}?OrderId={order.OrderId}";
            await Shell.Current.GoToAsync(route);
        }
    }
}

