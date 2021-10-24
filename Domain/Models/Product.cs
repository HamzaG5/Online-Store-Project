﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Product
    {
        public Guid ProductId { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public double Amount { get; set; }

        public string PartitionKey { get; set; }
    }
}
