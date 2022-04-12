using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElectronicsStore.Models;

namespace ElectronicsStore.Services
{
    public interface ICartDataStore<T>
    {
        Task<IEnumerable<Cart>> GetCartItemsAsync();
        Task<Cart> GetCartItemAsync(int cartItemId);
        Task<Cart> GetCartItemAsync(Product product);
        Task AddToCartAsync(Cart cartItem);
        Task UpdateCartAsync(Cart cartItem);
        Task DeleteCartAsync(Cart cartItem);
        Task DeleteWholeCartAsync();
    }
}
