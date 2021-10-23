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
    public class ForumHttpTrigger
    {
		private readonly IForumService _forumService;

        public ForumHttpTrigger(IForumService forumService)
        {
            _forumService = forumService;
        }

        [Function("AddReview")]
        public async Task<HttpResponseData> AddReviewAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "forum/review")] HttpRequestData req,
            FunctionContext executionContext)
        {
            // get request data
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var forumReview = JsonConvert.DeserializeObject<Forum>(requestBody);

            // create response
            var response = req.CreateResponse();

            await response.WriteAsJsonAsync(await _forumService.AddReview(forumReview));
            response.StatusCode = HttpStatusCode.Created;

            return response;
        }
    }
}
