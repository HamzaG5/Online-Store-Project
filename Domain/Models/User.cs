using System;
using System.Text;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class User
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        [JsonIgnore] 
        public string PartitionKey { get; set; }
    }
}
