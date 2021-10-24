using Domain.Models;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ForumService : IForumService
    {
        private readonly IOnlineStoreReadRepository<Forum> _forumReadRepository;
        private readonly IOnlineStoreWriteRepository<Forum> _forumWriteRepository;

        public ForumService(IOnlineStoreReadRepository<Forum> forumReadRepository, 
            IOnlineStoreWriteRepository<Forum> forumWriteRepository)
        {
            _forumReadRepository = forumReadRepository;
            _forumWriteRepository = forumWriteRepository;
        }

        public async Task<Forum> AddReview(Forum forumReview)
        {
            if (forumReview == null)
            {
                throw new ArgumentNullException("Forum must not be null.");
            }

            forumReview.ForumId = Guid.NewGuid();
            forumReview.PartitionKey = forumReview.ForumId.ToString();
            return await _forumWriteRepository.AddAsync(forumReview);
        }
    }
}
