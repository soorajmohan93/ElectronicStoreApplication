using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using ElectronicsStore.Models;
using Newtonsoft.Json;

namespace ElectronicsStore.Services
{
    public class CartDataStoreBlobStorageJson: ICartDataStore<Cart>
    {
        private readonly BlobServiceClient service = new BlobServiceClient(ConnectionString);

        private static string ConnectionString => Constants.ConnectionString;

        private static string Container => "data";

        private static string FileName => "Cart.json";

        public CartDataStoreBlobStorageJson()
        {
        }

        public async Task WriteFile(List<Cart> cartItems)
        {
            var jsonString = JsonConvert.SerializeObject(cartItems);

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(jsonString);
            writer.Flush();
            stream.Position = 0;

            BlobClient blob = service.GetBlobContainerClient(Container).GetBlobClient(FileName);
            await blob.UploadAsync(stream, overwrite: true);
        }

        public async Task<List<Cart>> ReadFile()
        {
            BlobClient blob = service.GetBlobContainerClient(Container).GetBlobClient(FileName);

            if (blob.Exists())
            {
                var stream = new MemoryStream();
                await blob.DownloadToAsync(stream);

                stream.Position = 0;

                var jsonString = new StreamReader(stream).ReadToEnd();

                var cartItems = JsonConvert.DeserializeObject<List<Cart>>(jsonString);

                return cartItems;
            }

            else
            {
                var defaultCartItems = new List<Cart>();
                await WriteFile(defaultCartItems);

                return defaultCartItems;
            }
        }


        



        public async Task<IEnumerable<Cart>> GetCartItemsAsync()
        {
            return await ReadFile();
        }

        public async Task<Cart> GetCartItemAsync(int cartItemId)
        {
            var cartItems = await ReadFile();
            var cartItem = cartItems.Find(c => c.CartItemId == cartItemId);
            return cartItem;
        }

        public async Task AddToCartAsync(Cart cartItem)
        {
            var cartItems = await ReadFile();
            if (cartItems == null)
            {
                cartItems = new List<Cart>() { new Cart()
                { CartItemId = cartItem.CartItemId , CartItemQty = cartItem.CartItemQty, CartProductDetails = cartItem.CartProductDetails} };
            }
            else
            {
                cartItems.Add(cartItem);
            }
            
            await WriteFile(cartItems);
        }

        public async Task UpdateCartAsync(Cart cartItem)
        {
            var cartItems = await ReadFile();
            cartItems[cartItems.FindIndex(c => c.CartItemId == cartItem.CartItemId)] = cartItem;
            await WriteFile(cartItems);
        }

        public async Task DeleteCartAsync(Cart cartItem)
        {
            var cartItems = await ReadFile();
            var remove = cartItems.Find(c => c.CartItemId == cartItem.CartItemId);
            cartItems.Remove(remove);
            await WriteFile(cartItems);
        }

        public async Task<Cart> GetCartItemAsync(Product product)
        {
            var cartItems = await ReadFile();
            var cartItem = cartItems.Find(c => c.CartProductDetails.ProductId == product.ProductId);
            return cartItem;
        }

        public async Task DeleteWholeCartAsync()
        {
            var cartItems = new List<Cart>();
            await WriteFile(cartItems);
        }
    }
}
