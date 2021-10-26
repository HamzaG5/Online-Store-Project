using Domain.DTO;
using Domain.Models;
using Infrastructure.Repositories;
using Infrastructure.Services.ProductService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.ForumService
{
    public class ForumService : IForumService
    {
        private readonly IProductService _productService;
        private readonly IOnlineStoreReadRepository<Review> _forumReadRepository;
        private readonly IOnlineStoreWriteRepository<Review> _forumWriteRepository;

        public ForumService(IOnlineStoreReadRepository<Review> forumReadRepository, 
            IOnlineStoreWriteRepository<Review> forumWriteRepository, IProductService productService)
        {
            _forumReadRepository = forumReadRepository;
            _forumWriteRepository = forumWriteRepository;
            _productService = productService;
        }

        public async Task<List<Review>> GetAllReviewsAsync()
        {
            var reviews = await _forumReadRepository.GetAll().ToListAsync();
            return reviews;
        }

        public async Task<Review> AddReview(ReviewDTO reviewDTO)
        {
            if (reviewDTO == null)
            {
                throw new ArgumentNullException("Review must not be null. Missing request body.");
            }

            await _productService.GetProductByIdAsync(reviewDTO.ProductId); // check if product exists

            Review review = new Review()
            {
                ReviewId = Guid.NewGuid(),
                ProductId = Guid.Parse(reviewDTO.ProductId),
                Rating = reviewDTO.Rating > 0 ? reviewDTO.Rating : throw new ArgumentException($"Invalid {nameof(reviewDTO.Rating)} provided."),
                Description = !string.IsNullOrWhiteSpace(reviewDTO.Description) ? reviewDTO.Description : throw new ArgumentNullException($"No {nameof(reviewDTO.Description)} was provided."),
                PartitionKey = reviewDTO.ProductId
            };

            return await _forumWriteRepository.AddAsync(review);
        }
    }
}
