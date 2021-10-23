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

        [Function("Orders")]
        public HttpResponseData Orders([HttpTrigger(AuthorizationLevel.Function, "get", Route = "orders")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("Function1");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

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
    }
}
