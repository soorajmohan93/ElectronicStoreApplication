using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElectronicsStore.Models;
using ElectronicsStore.Services;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace ElectronicsStore.ViewModels
{
    [QueryProperty(nameof(ProductCategory), nameof(ProductCategory))]
    public class ProductListPageViewModel: ElectronicStoreManagerBase
    {
        public ObservableRangeCollection<Product> Products { get; set; }
        public ObservableRangeCollection<Product> DisplayedProducts { get; set; }

        public AsyncCommand RefreshCommand { get; }

        public AsyncCommand<Product> TappedCommand { get; }

        private string productCategory;

        public string ProductCategory
        {
            get
            {
                return productCategory;
            }

            set
            {
                productCategory = value;
                ResultText = "Showing Products for " + productCategory;
                Products.Clear();
                LoadDisplayedProducts(productCategory);
            }
        }



        public ProductListPageViewModel()
        {
            Products = new ObservableRangeCollection<Product>();
            DisplayedProducts = new ObservableRangeCollection<Product>();
            
            RefreshCommand = new AsyncCommand(Refresh);
            TappedCommand = new AsyncCommand<Product>(OpenProductDetails);
        }

        private string resultText;

        public string ResultText
        {
            get => resultText;
            set => SetProperty(ref resultText, value);
        }

        public async Task Refresh()
        {
            IsBusy = true;

            Products.Clear();
            LoadDisplayedProducts(productCategory);

            IsBusy = false;
        }


        public async void LoadDisplayedProducts (string category)
        {
            IEnumerable<Product> products = await ProductDataStore.GetProductsAsync();
            foreach (Product product in products)
            {
                if (product.ProductCategory == category)
                    Products.Add(product);
            }
        }

        public async Task OpenProductDetails(Product product)
        {
            var route = $"{nameof(Views.ProductDetailPage)}?ProductId={product.ProductId}";
            await Shell.Current.GoToAsync(route);
        }

    }
}
