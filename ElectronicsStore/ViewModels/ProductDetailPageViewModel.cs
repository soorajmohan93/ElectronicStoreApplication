using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicsStore.Models;
using MvvmHelpers.Commands;
using Xamarin.Forms;

namespace ElectronicsStore.ViewModels
{
    [QueryProperty(nameof(ProductId), nameof(ProductId))]
    public class ProductDetailPageViewModel: ElectronicStoreManagerBase
    {
        private int productId;
        public int ProductId
        {
            get
            {
                return productId;
            }
            set
            {
                productId = value;
                LoadProduct(value);
            }
        }

        private Product product;
        public Product Product
        {
            get => product;
            set => SetProperty(ref product, value);
        }

        private Cart cart;
        public Cart Cart
        {
            get => cart;
            set => SetProperty(ref cart, value);
        }

        private string productName;
        public string ProductName
        {
            get => productName;
            set => SetProperty(ref productName, value);
        }

        private string productDescription;
        public string ProductDescription
        {
            get => productDescription;
            set => SetProperty(ref productDescription, value);
        }

        private int quantity;
        public int Quantity
        {
            get => quantity;
            set => SetProperty(ref quantity, value);
        }

        public AsyncCommand ReduceCommand { get; }
        public AsyncCommand IncreaseCommand { get; }
        public AsyncCommand AddToCartCommand { get; }



        public ProductDetailPageViewModel()
        {
            ReduceCommand = new AsyncCommand(Reduce);
            IncreaseCommand = new AsyncCommand(Increase);
            AddToCartCommand = new AsyncCommand(AddToCart);
        }

        private string productPrice;
        public string ProductPrice
        {
            get => productPrice;
            set => SetProperty(ref productPrice, value);
        }


        public async void LoadProduct(int id)
        {
            Product = await ProductDataStore.GetProductAsync(id);
            Cart = await CartDataStore.GetCartItemAsync(product);
            ProductName = product.ProductName;
            ProductDescription = product.ProductDesc;
            ProductPrice = "CAD " + product.ProductPrice;
            if (Cart != null)
            {
                Quantity = Cart.CartItemQty;
            }
        }

        public async Task Reduce()
        {
            if (quantity>0)
            {
                Quantity -= 1;
            }
        }

        public async Task Increase()
        {
            Quantity += 1;
        }

        public async Task AddToCart()
        {
            if (Cart != null)
            {
                if (Quantity == 0)
                {
                    await CartDataStore.DeleteCartAsync(Cart);
                }
                else
                {
                    Cart.CartItemQty = Quantity;
                    await CartDataStore.UpdateCartAsync(Cart);
                }
                
            }
            else
            {
                if (Quantity > 0)
                {
                    List<Cart> itemsInCart = (List<Cart>)await CartDataStore.GetCartItemsAsync();
                    int cartItemId = 1;
                    if (itemsInCart != null)
                        foreach (var item in itemsInCart)
                        {
                            if (item.CartItemId == cartItemId)
                                cartItemId += 1;
                            else
                                break;
                        }

                    Cart cartItem = new Cart() { CartItemId = cartItemId, CartItemQty = Quantity, CartProductDetails = Product };
                    await CartDataStore.AddToCartAsync(cartItem);
                }
            }
            
            await Shell.Current.Navigation.PopAsync();
        }
    }
}
