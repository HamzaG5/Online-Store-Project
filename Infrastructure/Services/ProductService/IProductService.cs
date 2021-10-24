using Domain.DTO;
using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services.ProductService
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();

        Task<Product> GetProductByIdAsync(string productId);

        Task<Product> AddProduct(ProductDTO productDTO);
    }
}