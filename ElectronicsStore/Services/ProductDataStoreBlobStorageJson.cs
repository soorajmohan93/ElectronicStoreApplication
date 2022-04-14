using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using ElectronicsStore.Models;
using Newtonsoft.Json;

namespace ElectronicsStore.Services
{
    public class ProductDataStoreBlobStorageJson: IProductDataStore<Product>
    {
        private readonly BlobServiceClient service = new BlobServiceClient(ConnectionString);

        private static string ConnectionString => "DefaultEndpointsProtocol=https;AccountName=soorajjobmanager;AccountKey=yyCKVbnBK7pMN9hzKyqSK1sRwwELDoqXSPDxjPLTwOxGvccqysrEigv2hSGqrKiCyLuxtTEsB2TL+ASthcQWOA==;EndpointSuffix=core.windows.net";

        private static string Container => "data";

        private static string FileName => "Products.json";

        public ProductDataStoreBlobStorageJson()
        {
        }

        public async Task WriteFile(List<Product> products)
        {
            var jsonString = JsonConvert.SerializeObject(products);

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(jsonString);
            writer.Flush();
            stream.Position = 0;

            BlobClient blob = service.GetBlobContainerClient(Container).GetBlobClient(FileName);
            await blob.UploadAsync(stream);
        }

        public async Task<List<Product>> ReadFile()
        {
            BlobClient blob = service.GetBlobContainerClient(Container).GetBlobClient(FileName);

            if (blob.Exists())
            {
                var stream = new MemoryStream();
                await blob.DownloadToAsync(stream);

                stream.Position = 0;

                var jsonString = new StreamReader(stream).ReadToEnd();

                var products = JsonConvert.DeserializeObject<List<Product>>(jsonString);

                return products;
            }

            else
            {
                var defaultProducts = GetDefaultProducts();
                await WriteFile(defaultProducts);

                return defaultProducts;
            }
        }

        private List<Product> GetDefaultProducts()
        {
            var products = new List<Product>()
            {
                new Product() { ProductId = 1, ProductName = "Apple MacBook Pro 13", ProductPrice = 1899.00M, ProductCategory = "Laptops", ProductDesc = "13 inch laptop with M1 Silicon chip" },
                new Product() { ProductId = 2, ProductName = "Apple MacBook Pro 14", ProductPrice = 2199.00M, ProductCategory = "Laptops", ProductDesc = "14 inch laptop with M1 Silicon chip" },
                new Product() { ProductId = 3, ProductName = "Apple MacBook Pro 16", ProductPrice = 2399.00M, ProductCategory = "Laptops", ProductDesc = "16 inch laptop with M1 Silicon chip" },
                new Product() { ProductId = 4, ProductName = "Apple Mac", ProductPrice = 1799.00M, ProductCategory = "Desktops", ProductDesc = "Apple Mac with M1 Silicon chip" },
                new Product() { ProductId = 5, ProductName = "Lenovo ThinkCentre", ProductPrice = 1099.00M, ProductCategory = "Desktops", ProductDesc = "Levono PC" },
                new Product() { ProductId = 6, ProductName = "Dell Desktop Computer", ProductPrice = 1299.00M, ProductCategory = "Desktops", ProductDesc = "Dell PC" },
                new Product() { ProductId = 7, ProductName = "Apple IPhone 13 Pro", ProductPrice = 1000.00M, ProductCategory = "Mobile Phones", ProductDesc = "Apple Phone" },
                new Product() { ProductId = 8, ProductName = "Samsung Galaxy S22", ProductPrice = 899.00M, ProductCategory = "Mobile Phones", ProductDesc = "Samsung Phone" },
                new Product() { ProductId = 9, ProductName = "OnePlus 10 Pro", ProductPrice = 799.00M, ProductCategory = "Mobile Phones", ProductDesc = "OnePlus Phone" },
                new Product() { ProductId = 10, ProductName = "Sony PS5", ProductPrice = 1200.00M, ProductCategory = "Gaming Consoles", ProductDesc = "Playstation 5 from Sony" },
                new Product() { ProductId = 11, ProductName = "Microsoft XBox X", ProductPrice = 1000.00M, ProductCategory = "Gaming Consoles", ProductDesc = "Latest Xbox console" },
                new Product() { ProductId = 12, ProductName = "Nintendo Switch", ProductPrice = 899.00M, ProductCategory = "Gaming Consoles", ProductDesc = "Portable console" },
                new Product() { ProductId = 13, ProductName = "Sony WH-1000XM4", ProductPrice = 348.00M, ProductCategory = "Accessories", ProductDesc = "Sony Headphones" },
                new Product() { ProductId = 14, ProductName = "Bose QuietComfort 45", ProductPrice = 379.00M, ProductCategory = "Accessories", ProductDesc = "Noise Cancelling Headphones from Bose" },
                new Product() { ProductId = 15, ProductName = "Apple Watch Series 3", ProductPrice = 249.00M, ProductCategory = "Accessories", ProductDesc = "Aluminium Case with Black Sport Band" }
        };

            return products;
        }

        public async Task AddProductAsync(Product product)
        {
            var products = await ReadFile();
            products.Add(product);
            await WriteFile(products);
        }

        public async Task DeleteProductAsync(Product product)
        {
            var products = await ReadFile();
            var remove = products.Find(p => p.ProductId == product.ProductId);
            products.Remove(remove);
            await WriteFile(products);
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            var products = await ReadFile();
            var product = products.Find(p => p.ProductId == productId);
            return product;
        }


        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await ReadFile();
        }

        public async Task UpdateProductAsync(Product product)
        {
            var products = await ReadFile();
            products[products.FindIndex(p => p.ProductId == product.ProductId)] = product;
            await WriteFile(products);
        }
    }
}

