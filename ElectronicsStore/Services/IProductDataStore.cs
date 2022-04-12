using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElectronicsStore.Models;

namespace ElectronicsStore.Services
{
    public interface IProductDataStore<T>
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductAsync(int productId);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Product product);
    }
}
