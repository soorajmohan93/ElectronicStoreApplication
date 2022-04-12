using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Threading.Tasks;
using ElectronicsStore.Models;
using ElectronicsStore.Services;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Xamarin.Forms;

namespace ElectronicsStore.ViewModels
{
    public class CartListPageViewModel : ElectronicStoreManagerBase
    {
        public ObservableRangeCollection<Cart> CartItems { get; set; }

        private string cartValue;
        public string CartValue
        {
            get => cartValue;
            set => SetProperty(ref cartValue, value);
        }

        private string cartIsEmpty;
        public string CartIsEmpty
        {
            get => cartIsEmpty;
            set => SetProperty(ref cartIsEmpty, value);
        }

        public AsyncCommand RefreshCommand { get; }

        public AsyncCommand PageAppearingCommand { get; }
        public AsyncCommand PageDisappearingCommand { get; }

        public AsyncCommand PlaceOrderCommand { get; }

        public AsyncCommand<Cart> TappedCommand { get; }


        public CartListPageViewModel()
        {
            
            CartItems = new ObservableRangeCollection<Cart>();
            RefreshCommand = new AsyncCommand(Refresh);

            PageAppearingCommand = new AsyncCommand(PageAppearing);
            PageDisappearingCommand = new AsyncCommand(PageDisappearing);

            PlaceOrderCommand = new AsyncCommand(PlaceOrder);

            TappedCommand = new AsyncCommand<Cart>(EditCartItem);
        }

        async Task PageAppearing()
        {
            CartItems.Clear();
            LoadCartItems();
        }

        async Task PageDisappearing()
        {
        }


        public async Task Refresh()
        {
            IsBusy = true;

            CartItems.Clear();
            LoadCartItems();

            IsBusy = false;
        }

        public async void LoadCartItems()
        {
            IEnumerable<Cart> cartItems = await CartDataStore.GetCartItemsAsync();
            CartItems.AddRange(cartItems);

            if (CartItems.Count == 0)
            {
                CartIsEmpty = "Cart Is Empty";
                CartValue = "Cart is Empty";
            }
                
                
            else
            {
                CartIsEmpty = null;
                decimal totalValue = 0.00M;
                foreach (var cartItem in CartItems)
                {
                    decimal itemValue = cartItem.CartItemQty * cartItem.CartProductDetails.ProductPrice;
                    totalValue += itemValue;
                }
                CartValue = $"Place Order (CAD {totalValue})";
            }
                
        }

        public async Task EditCartItem(Cart cartItem)
        {
            var route = $"{nameof(Views.ProductDetailPage)}?ProductId={cartItem.CartProductDetails.ProductId}";
            await Shell.Current.GoToAsync(route);
        }

        public async Task PlaceOrder()
        {
            if (CartItems.Count != 0)
            {
                List<Order> orders = (List<Order>)await OrderDataStore.GetOrdersAsync();
                int orderId = 1;
                if (orders != null)
                {
                    foreach (var item in orders)
                    {
                        if (item.OrderId == orderId)
                            orderId += 1;
                        else
                            break;
                    }
                }

                decimal totalValue = 0.00M;
                foreach (var cartItem in CartItems)
                {
                    decimal itemValue = cartItem.CartItemQty * cartItem.CartProductDetails.ProductPrice;
                    totalValue += itemValue;
                }

                Order order = new Order() { OrderId = orderId, OrderValue = totalValue };
                List<OrderItem> orderItems = new List<OrderItem>();
                int orderItemId = 1;
                foreach (var cartItem in CartItems)
                {
                    orderItems.Add(new OrderItem()
                    {
                        Order = order,
                        OrderItemId = orderItemId,
                        OrderItemName = cartItem.CartProductDetails.ProductName,
                        OrderItemQty = cartItem.CartItemQty,
                        OrderItemValue = cartItem.CartProductDetails.ProductPrice * cartItem.CartItemQty
                    }) ;
                    orderItemId += 1;
                }   
                DateTime currentTimestamp = DateTime.Now;
                var culture = new CultureInfo("en-US");
                OrderTracking orderTracking = new OrderTracking() { Order = order, OrderTrackingStatus = "Order Placed", OrderTrackingTimeStamp = currentTimestamp.ToString(culture) };
                await OrderDataStore.AddOrderAsync(order);
                await OrderItemDataStore.AddOrderItemsAsync(orderItems);
                await OrderTrackingDataStore.AddOrderTrackingAsync(orderTracking);
                await CartDataStore.DeleteWholeCartAsync();
            }

            
            await Shell.Current.GoToAsync("//WelcomePage", false);
        }
    }
}
