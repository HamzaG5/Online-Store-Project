using System;

namespace Domain.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        
        public Guid ProductId { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime ShippingDate { get; set; }

        public bool Shipped { get; set; }

        public string PartitionKey { get; set; }

        public Order()
        {
        }
    }
}
