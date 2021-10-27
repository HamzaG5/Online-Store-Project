using Domain.DTO;
using Domain.Models;
using HttpMultipartParser;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Services.ProductService
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();

        Task<Product> GetProductByIdAsync(string productId);

        Task<Product> AddProduct(ProductDTO productDTO);

        Task<Product> UploadProductImage(string productId, FilePart imageFile);

        Task DeleteProductAsync(string productId);
    }
}