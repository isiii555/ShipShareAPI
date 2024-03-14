using ShipShareAPI.Application.Dto.Review;
using ShipShareAPI.Domain.Entities;


namespace ShipShareAPI.Application.Interfaces.Repositories
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetAllReviews();
        Task<List<Review>> GetAllReviewsAdmin();
        Task<ReviewDto> UpdateReview(Guid reviewId, UpdateReviewRequest updateReviewRequest);
        Task<bool> SetStatusReview(Guid reviewId, bool status);
        Task<ReviewDto> CreateReview(Guid postId,CreateReviewRequest createReviewRequest);
        Task<List<Review>> GetUserReviews(Guid userId);
        Task<bool> DeleteReview(Guid reviewId);
    }
}
