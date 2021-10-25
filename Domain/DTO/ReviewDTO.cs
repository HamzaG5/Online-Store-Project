using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ReviewDTO
    {
        public string ProductId { get; set; }

        public int Rating { get; set; }

        public string Description { get; set; }
    }
}
