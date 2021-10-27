using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Domain.DTO;
using HttpMultipartParser;
using Infrastructure.Services.ProductService;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;

namespace OnlineStoreProject.Controller
{
    public class ProductController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Function("GetAllProducts")]
        public async Task<HttpResponseData> GetAllProductsAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var response = req.CreateResponse();

            await response.WriteAsJsonAsync(await _productService.GetAllProductsAsync());

            return response;
        }

        [Function("CreateProduct")]
        public async Task<HttpResponseData> CreateProductAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "products")] HttpRequestData req,
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

        [Function("UploadProductImage")]
        public async Task<HttpResponseData> UploadProductImageAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "products/image/{productId}")] HttpRequestData req,
            FunctionContext executionContext, string productId)
        {
            // get request data
            var parsedFormBody = MultipartFormDataParser.ParseAsync(req.Body);
            var file = parsedFormBody.Result.Files[0];
           
            // create response
            var response = req.CreateResponse(HttpStatusCode.Created);

            var product = await _productService.UploadProductImage(productId, file);

            await response.WriteStringAsync($"Image product uploaded. Image URL: {product.Images.LastOrDefault().ImageUrl}");

            return response;
        }

        [Function("DeleteProduct")]
        public async Task<HttpResponseData> DeleteProductAsync([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "products/{productId}")] HttpRequestData req,
            FunctionContext executionContext, string productId)
        {
            // create response
            HttpResponseData response = req.CreateResponse(HttpStatusCode.Accepted);

            await _productService.DeleteProductAsync(productId);
            await response.WriteStringAsync("The product was successfully removed.");

            return response;
        }
    }
}
