using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Models;
using Infrastructure.Services.ForumService;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace OnlineStoreProject
{
    public class ForumController
    {
		private readonly IForumService _forumService;

        public ForumController(IForumService forumService)
        {
            _forumService = forumService;
        }

        [Function("GetAllReviews")]
        public async Task<HttpResponseData> GetAllReviewsAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "forum/reviews")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var response = req.CreateResponse();

            await response.WriteAsJsonAsync(await _forumService.GetAllReviewsAsync());

            return response;
        }

        [Function("AddReview")]
        public async Task<HttpResponseData> AddReviewAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "forum/reviews")] HttpRequestData req,
            FunctionContext executionContext)
        {
            // get request data
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var reviewDTO = JsonConvert.DeserializeObject<ReviewDTO>(requestBody);

            // create response
            var response = req.CreateResponse();

            await response.WriteAsJsonAsync(await _forumService.AddReview(reviewDTO));
            response.StatusCode = HttpStatusCode.Created;

            return response;
        }
    }
}
