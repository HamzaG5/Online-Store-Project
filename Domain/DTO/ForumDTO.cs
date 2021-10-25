using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ForumDTO
    {
        public string UserId { get; set; }

        public string ProductId { get; set; }

        public int Rating { get; set; }

        public string Review { get; set; }
    }
}
