
using System.Threading.Tasks;
using ElectronicsStore.Models;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace ElectronicsStore.ViewModels
{
    public class WelcomePageViewModel: ElectronicStoreManagerBase
    {
        public AsyncCommand<ProductCategory> TappedCommand { get; }

        public ObservableRangeCollection<ProductCategory> ProductCategories { get; set; }

        public WelcomePageViewModel()
        {
            Title = "Welcome To Electronics Store";
            ProductCategories = new ObservableRangeCollection<ProductCategory>();
            FetchCategories();

            TappedCommand = new AsyncCommand<ProductCategory>(DisplayProductList);
        }

        public async Task DisplayProductList(ProductCategory category)
        {
            var route = $"{nameof(Views.ProductListPage)}?ProductCategory={category.CategoryName}";
            await Shell.Current.GoToAsync(route);
        }

        public async void FetchCategories()
        {
            ProductCategories.Add(new ProductCategory() { CategoryName = "Laptops" });
            ProductCategories.Add(new ProductCategory() { CategoryName = "Desktops" });
            ProductCategories.Add(new ProductCategory() { CategoryName = "Mobile Phones" });
            ProductCategories.Add(new ProductCategory() { CategoryName = "Gaming Consoles" });
            ProductCategories.Add(new ProductCategory() { CategoryName = "Accessories" });
        }
    }
}
