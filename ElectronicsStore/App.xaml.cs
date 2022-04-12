using System;
using ElectronicsStore.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ElectronicsStore
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<ProductDataStoreBlobStorageJson>();
            DependencyService.Register<CartDataStoreBlobStorageJson>();
            DependencyService.Register<OrderDataStoreBlobStorageJson>();
            DependencyService.Register<OrderItemDataStoreBlobStorageJson>();
            DependencyService.Register<OrderTrackingDataStoreBlobStorageJson>();

            MainPage = new AppShell();
        }

    }
}
