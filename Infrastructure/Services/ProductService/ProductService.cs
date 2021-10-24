using Domain.Models;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IOnlineStoreReadRepository<Product> _productReadRepository;
        private readonly IOnlineStoreWriteRepository<Product> _productriteRepository;

        public ProductService(IOnlineStoreReadRepository<Product> productReadRepository, 
            IOnlineStoreWriteRepository<Product> productWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productriteRepository = productWriteRepository;
        }

        public async Task<Product> AddProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException("Forum must not be null.");
            }

            return await _productriteRepository.AddAsync(product);
        }
    }
}
