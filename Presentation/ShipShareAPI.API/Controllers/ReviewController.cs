using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShipShareAPI.Application.Dto.Review;
using ShipShareAPI.Application.Interfaces.Repositories;

namespace ShipShareAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        public ReviewController(IReviewRepository reviewRepository)
        {
            _reviewRepository = Guard.Against.Null(reviewRepository);
        }

        [HttpPost("createReview/{postId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<ReviewDto>> CreateReview(Guid postId,CreateReviewRequest createReviewRequest)
        {
            var review = await _reviewRepository.CreateReview(postId, createReviewRequest);
            if (review is not null)
                return Ok(review);
            else
                return BadRequest();
        }

        [HttpPut("updateReview/{reviewId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<ReviewDto>> UpdateReview(Guid reviewId,UpdateReviewRequest updateReviewRequest)
        {
            var review = await _reviewRepository.UpdateReview(reviewId, updateReviewRequest);
            if (review is not null)
                return Ok(review);
            else 
                return BadRequest();
        }

        [HttpDelete("deleteReview/{reviewId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<bool>> DeleteReview(Guid reviewId)
        {
            var result = await _reviewRepository.DeleteReview(reviewId);
            return Ok(result);
        }

        [HttpGet("getAllReviews")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> GetAllReviews()
        {
            var result = await _reviewRepository.GetAllReviews();
            return Ok(result);
        }

    }
}
