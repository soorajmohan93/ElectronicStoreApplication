using System;
using System.Collections;
using System.Collections.Generic;

namespace ElectronicsStore.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public decimal OrderValue { get; set; }

        ICollection<OrderItem> OrderItems { get; set; } 
    }
}
