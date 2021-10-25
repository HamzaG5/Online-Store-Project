using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class Forum
    {
        public Guid ForumId { get; set; }

        public Guid? UserId { get; set; }

        public Guid ProductId { get; set; }

        public int Rating { get; set; }

        public string Review { get; set; }

        [JsonIgnore]
        public string PartitionKey { get; set; }
    }
}
