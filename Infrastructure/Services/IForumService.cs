using Domain;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IForumService
    {
        Task<Forum> AddReview(Forum forumReview);
    }
}