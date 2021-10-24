﻿using Domain.DTO;
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

        public async Task<List<Product>> GetAllProductsAsync()
        {
            var products = await _productReadRepository.GetAll().ToListAsync();
            return products;
        }

        public async Task<Product> GetProductByIdAsync(string productId)
        {
            try
            {
                Guid resultId = !string.IsNullOrWhiteSpace(productId) ? Guid.Parse(productId) : throw new ArgumentNullException("No Product ID was provided.");

                var product = await _productReadRepository.GetAll().FirstOrDefaultAsync(p => p.ProductId == resultId) ??
                    throw new InvalidOperationException($"Poduct does not exist. Incorrect Product ID: {productId} provided.");

                return product;
            }
            catch
            {
                throw new InvalidOperationException($"Invalid format of Product ID: {productId} provided.");
            }
        }

        private async Task<Product> GetProductByNameAsync(string productName)
        {
            productName = !string.IsNullOrWhiteSpace(productName) ? productName : throw new ArgumentNullException("No Product Name was provided.");

            var product = await _productReadRepository.GetAll().FirstOrDefaultAsync(p => p.ProductName == productName);
            return product;
        }

        public async Task<Product> AddProduct(ProductDTO productDTO)
        {
            if (productDTO == null)
            {
                throw new ArgumentNullException("Forum must not be null.");
            }

            var findProduct = await GetProductByNameAsync(productDTO.ProductName) !?? 
                throw new ArgumentException($"Product already exists with this name: {productDTO.ProductName}.");

            Product product = new Product()
            {
                ProductId = Guid.NewGuid(),
                ProductName = productDTO.ProductName,
                Description = !string.IsNullOrWhiteSpace(productDTO.Description) ? productDTO.Description : throw new ArgumentNullException($"{nameof(productDTO.Description)} must be provided."),
                ImageUrl = !string.IsNullOrWhiteSpace(productDTO.ImageUrl) ? productDTO.ImageUrl : throw new ArgumentNullException($"{nameof(productDTO.ImageUrl)} must be provided."),
                Amount = productDTO.Amount != 0 ? productDTO.Amount : throw new ArgumentException($"{nameof(productDTO.ImageUrl)} must be provided."),
                PartitionKey = productDTO.Amount.ToString()
            };

            return await _productriteRepository.AddAsync(product);
        }
    }
}
