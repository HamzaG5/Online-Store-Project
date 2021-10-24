using Domain.Models;
using System.Threading.Tasks;

namespace Infrastructure.Services.ForumService
{
    public interface IForumService
    {
        Task<Forum> AddReview(Forum forumReview);
    }
}