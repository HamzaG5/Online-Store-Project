using System.IO;
using System.Net;
using System.Threading.Tasks;
using Domain.DTO;
using Infrastructure.Services.ForumService;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
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
        public async Task<HttpResponseData> GetAllReviewsAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "forum/reviews")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var response = req.CreateResponse();

            await response.WriteAsJsonAsync(await _forumService.GetAllReviewsAsync());

            return response;
        }

        [Function("AddReview")]
        public async Task<HttpResponseData> AddReviewAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "forum/reviews")] HttpRequestData req,
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
