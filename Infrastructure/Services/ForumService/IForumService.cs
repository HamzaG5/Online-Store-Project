using Domain.DTO;
using Domain.Models;
using System.Threading.Tasks;

namespace Infrastructure.Services.ForumService
{
    public interface IForumService
    {
        Task<Review> AddReview(ReviewDTO forumReview);
    }
}