using System;
namespace ElectronicsStore.Models
{
    public class OrderTracking
    {
        public string OrderTrackingTimeStamp { get; set; }
        public string OrderTrackingStatus { get; set; }

        public Order Order { get; set; }
    }
}
