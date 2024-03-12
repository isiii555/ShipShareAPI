using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipShareAPI.Application.Interfaces.Repositories;
using ShipShareAPI.Domain.Entities;

namespace ShipShareAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly ISenderPostRepository _senderPostRepository;
        private readonly ITravellerPostRepository _travellerPostRepository;
        private readonly IReviewRepository _reviewRepository;

        public AdminController(ITravellerPostRepository travellerPostRepository, ISenderPostRepository senderPostRepository, IReviewRepository reviewRepository)
        {
            _travellerPostRepository = Guard.Against.Null(travellerPostRepository);
            _senderPostRepository = Guard.Against.Null(senderPostRepository);
            _reviewRepository = Guard.Against.Null(reviewRepository);
        }

        [HttpGet("getAllSenderPosts")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<SenderPost>>> GetAllSenderPosts()
        {
            return await _senderPostRepository.GetAllPostsAdmin();
        }

        [HttpGet("getAllTravellerPosts")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<TravellerPost>>> GetAllTravellerPosts()
        {
            return await _travellerPostRepository.GetAllPostsAdmin();
        }

        [HttpGet("getAllReviews")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Review>>> GetAllReviews()
        {
            return await _reviewRepository.GetAllReviewsAdmin();
        }

        [HttpPut("setStatusSenderPost/{postId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> SetStatusSenderPost(Guid postId, [FromBody] bool status)
        {
            var result = await _senderPostRepository.SetStatusSenderPost(postId,status);
            return result ? Ok(result) : BadRequest();
        }

        [HttpPut("setStatusTravellerPost/{postId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> SetStatusTravellerPost(Guid postId, [FromBody] bool status)
        {
            var result = await _travellerPostRepository.SetStatusTravellerPost(postId, status);
            return result ? Ok(result) : BadRequest();
        }

        [HttpPut("setStatusReview/{reviewId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> SetStatusReview(Guid reviewId, [FromBody] bool status)
        {
            var result = await _reviewRepository.SetStatusReview(reviewId, status);
            return result ? Ok(result) : BadRequest();
        }
    }
}
