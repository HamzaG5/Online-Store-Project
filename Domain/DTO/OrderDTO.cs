using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class OrderDTO
    {
        public string UserId { get; set; }

        public string ProductId { get; set; }

        public double PurchaseAmount { get; set; }

        public DateTime ShippingDate { get; set; }
    }
}
