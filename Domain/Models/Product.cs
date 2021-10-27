using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class Product
    {
        public Guid ProductId { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public List<Image> Images { get; set; }

        public double Amount { get; set; }

        [JsonIgnore]
        public string PartitionKey { get; set; }

        public Product()
        {
        }
    }
}
