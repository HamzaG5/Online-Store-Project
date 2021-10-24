using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Domain.DTO;
using Infrastructure.Services.ProductService;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace OnlineStoreProject.Controller
{
    public class ProductHttpTrigger
    {
        private readonly IProductService _productService;

        public ProductHttpTrigger(IProductService productService)
        {
            _productService = productService;
        }

        [Function("GetAllProducts")]
        public async Task<HttpResponseData> GetAllProductsAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "products")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var response = req.CreateResponse();

            await response.WriteAsJsonAsync(await _productService.GetAllProductsAsync());

            return response;
        }

        [Function("CreateProduct")]
        public async Task<HttpResponseData> CreateProductAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "products")] HttpRequestData req,
            FunctionContext executionContext)
        {
            // get request data
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var productDTO = JsonConvert.DeserializeObject<ProductDTO>(requestBody);

            // create response
            var response = req.CreateResponse();

            await response.WriteAsJsonAsync(await _productService.AddProduct(productDTO));
            response.StatusCode = HttpStatusCode.Created;

            return response;
        }
    }
}
