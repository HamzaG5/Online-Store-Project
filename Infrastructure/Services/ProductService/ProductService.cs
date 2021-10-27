using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Domain.DTO;
using Domain.Models;
using HttpMultipartParser;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services.ProductService
{
    public class ProductService : IProductService
    {

        private BlobServiceClient blobServiceClient;
        private BlobContainerClient containerClient;
        private readonly IOnlineStoreReadRepository<Product> _productReadRepository;
        private readonly IOnlineStoreWriteRepository<Product> _productWriteRepository;

        public ProductService(IOnlineStoreReadRepository<Product> productReadRepository, 
            IOnlineStoreWriteRepository<Product> productWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;

            blobServiceClient = new BlobServiceClient(Environment.GetEnvironmentVariable("BlobCreds:ConnectionString", EnvironmentVariableTarget.Process));
            containerClient = blobServiceClient.GetBlobContainerClient(Environment.GetEnvironmentVariable("BlobCreds:ContainerName", EnvironmentVariableTarget.Process));
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            var products = await _productReadRepository.GetAll().ToListAsync();
            return products;
        }

        public async Task<Product> GetProductByIdAsync(string productId)
        {
            Guid resultId;
            var isValid = !string.IsNullOrWhiteSpace(productId) ? Guid.TryParse(productId, out resultId) : throw new ArgumentNullException("No Product ID was provided.");
           
            if (!isValid)
            {
                throw new InvalidOperationException($"Invalid format of Product ID: {productId} provided.");
            }

            var product = await _productReadRepository.GetAll().FirstOrDefaultAsync(p => p.ProductId == resultId) ??
                throw new InvalidOperationException($"Poduct does not exist. Incorrect Product ID: {productId} provided.");

            return product;
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
                throw new ArgumentNullException("Product must not be null.");
            }

            if (await GetProductByNameAsync(productDTO.ProductName) != null)
            {
                throw new ArgumentException($"Product already exists with this name: {productDTO.ProductName}.");
            }

            Product product = new Product()
            {
                ProductId = Guid.NewGuid(),
                ProductName = productDTO.ProductName,
                Description = !string.IsNullOrWhiteSpace(productDTO.Description) ? productDTO.Description : throw new ArgumentNullException($"{nameof(productDTO.Description)} must be provided."),
                Amount = productDTO.Amount > 0 ? productDTO.Amount : throw new ArgumentException($"Invalid {nameof(productDTO.Amount)} provided.")
            };
            product.PartitionKey = product.ProductId.ToString();

            return await _productWriteRepository.AddAsync(product);
        }

        public async Task<Product> UploadProductImage(string productId, FilePart imageFile)
        {
            // check for correct content type
            _ = imageFile.ContentType == "image/jpeg" || imageFile.ContentType == "image/png" || imageFile.ContentType == "image/bmp"
                ? imageFile.ContentType : throw new InvalidOperationException("Invalid image type. Must be of type jpeg, png or bmp.");

            //check file size
            _ = imageFile.Data.Length > 0 ? imageFile.Data.Length : throw new ArgumentException("Invalid image size.");

            BlobClient blobClient = containerClient.GetBlobClient(imageFile.Name);
            await blobClient.UploadAsync(imageFile.Data, new BlobHttpHeaders { ContentType = imageFile.ContentType });
           
            var imageUrl = blobClient.Uri.AbsoluteUri; // get the URL of the uploaded image
            var image = new Image(imageFile.Name, imageUrl);

            var product = await GetProductByIdAsync(productId); // check if product exists
            product.Images.Add(image);

            return await _productWriteRepository.UpdateAsync(product); // update product
        }

        public async Task DeleteProductAsync(string productId)
        {
            Product product = await GetProductByIdAsync(productId);
            await _productWriteRepository.DeleteAsync(product);
        }
    }
}
