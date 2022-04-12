using System;
namespace ElectronicsStore.Models
{
    public class Cart
    {
        public int CartItemId { get; set; }

        public int CartItemQty { get; set; }

        public Product CartProductDetails { get; set; }
    }
}
