using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using OnlineStoreProject.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStoreProject.ErrorHandlerMiddleware
{
    public class GlobalErrorHandler : IFunctionsWorkerMiddleware
    {
        private ILogger Logger { get; }
        public GlobalErrorHandler(ILogger<GlobalErrorHandler> Logger)
        {
            this.Logger = Logger;
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error found in endpoint {context.FunctionDefinition.Name}: {ex}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(FunctionContext context, Exception exception)
        {
            var req = context.GetHttpRequestData();

            HttpResponseData response = req.CreateResponse();

            var responseData = new
            {
                Status = exception.GetBaseException() switch
                {
                    ArgumentNullException => HttpStatusCode.BadRequest,
                    ArgumentException => HttpStatusCode.BadRequest,
                    InvalidOperationException => HttpStatusCode.BadRequest,
                    NullReferenceException => HttpStatusCode.BadRequest,
                    _ => HttpStatusCode.InternalServerError
                },
                Message = exception.InnerException != null ? exception.InnerException.Message : exception.Message
            };

            await response.WriteAsJsonAsync(responseData);
            response.StatusCode = responseData.Status;

            context.InvokeResult(response);
        }
    }
}
