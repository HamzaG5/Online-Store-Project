using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class OrderDTO
    {
        public Guid ProductId { get; set; }

        public DateTime ShippingDate { get; set; }
    }
}
