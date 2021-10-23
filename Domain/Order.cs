using System;

namespace Domain
{
    public class Order
    {
        public Guid OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime ShippingDate { get; set; }

        public string PartitionKey { get; set; }

    }
}
