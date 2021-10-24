using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Forum
    {
        public Guid ForumId { get; set; }

        public Guid? UserId { get; set; }

        public Guid ProductId { get; set; }

        public int Rating { get; set; }

        public string Review { get; set; }

        public string PartitionKey { get; set; }
    }
}
