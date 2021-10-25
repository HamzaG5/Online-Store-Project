using System;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }

        public Guid UserId { get; set; }

        public Guid ProductId { get; set; }

        public double PurchaseAmount { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime ShippingDate { get; set; }

        public bool Shipped { get; set; }

        [JsonIgnore]
        public string PartitionKey { get; set; }

        public Order()
        {
        }
    }
}
