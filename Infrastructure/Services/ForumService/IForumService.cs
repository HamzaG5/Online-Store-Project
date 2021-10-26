using Domain.DTO;
using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services.ForumService
{
    public interface IForumService
    {
        Task<List<Review>> GetAllReviewsAsync();

        Task<Review> AddReview(ReviewDTO forumReview);
    }
}