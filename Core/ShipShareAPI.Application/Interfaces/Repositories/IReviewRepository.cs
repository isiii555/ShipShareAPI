using ShipShareAPI.Application.Dto.Review;
using ShipShareAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Interfaces.Repositories
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetAllReviews();
        Task<ReviewDto> UpdateReview(Guid reviewId, UpdateReviewRequest updateReviewRequest);
        Task<ReviewDto> CreateReview(Guid postId,CreateReviewRequest createReviewRequest);
        Task<bool> DeleteReview(Guid reviewId);
    }
}
