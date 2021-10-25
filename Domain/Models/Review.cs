using System;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class Review
    {
        public Guid ReviewId { get; set; }

        public Guid ProductId { get; set; }

        public int Rating { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public string PartitionKey { get; set; }
    }
}