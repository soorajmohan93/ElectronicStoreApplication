using System;
using System.Collections.Generic;
using ElectronicsStore.Views;
using Xamarin.Forms;

namespace ElectronicsStore
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            //Routing.RegisterRoute(nameof(WelcomePage), typeof(WelcomePage));
            Routing.RegisterRoute(nameof(ProductListPage), typeof(ProductListPage));
            Routing.RegisterRoute(nameof(ProductDetailPage), typeof(ProductDetailPage));
            Routing.RegisterRoute(nameof(OrderItemPage), typeof(OrderItemPage));
            Routing.RegisterRoute(nameof(OrderTrackingPage), typeof(OrderTrackingPage));
        }

    }
}
