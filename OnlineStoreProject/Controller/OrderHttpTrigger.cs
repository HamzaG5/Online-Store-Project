using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace OnlineStoreProject
{
    public class OrderHttpTrigger
    {
		private readonly IOrderService _orderService;

        public OrderHttpTrigger(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Function("GetAllOrders")]
        public async Task<HttpResponseData> GetAllOrdersAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "orders")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var response = req.CreateResponse();

            await response.WriteAsJsonAsync(await _orderService.GetAllOrdersAsync());

            return response;
        }

        [Function("CreateOrder")]
        public async Task<HttpResponseData> CreateOrderAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "orders")] HttpRequestData req,
            FunctionContext executionContext)
        {
            // get request data
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var order = JsonConvert.DeserializeObject<Order>(requestBody);

            // create response
            var response = req.CreateResponse();

            await response.WriteAsJsonAsync(await _orderService.AddOrder(order));
            response.StatusCode = HttpStatusCode.Created;

            return response;
        }

        [Function("ShipOrder")]
        public async Task<HttpResponseData> ShipOrderAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "orders/{orderId}/ship")] HttpRequestData req,
            FunctionContext executionContext, string orderId)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(await _orderService.ShipOrder(orderId));

            return response;
        }
    }
}
