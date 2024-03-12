using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipShareAPI.Application.Dto.Post.TravellerPost;
using ShipShareAPI.Application.Interfaces.Repositories;
using ShipShareAPI.Domain.Entities;

namespace ShipShareAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TravellerPostController : ControllerBase
    {
        private readonly ITravellerPostRepository _travellerPostsRepository;

        public TravellerPostController(ITravellerPostRepository travellerPostsRepository)
        {
            _travellerPostsRepository = Guard.Against.Null(travellerPostsRepository);
        }

        [HttpGet("getAllTravellerPosts")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllTravellerPostsAsync()
        {
            var posts = await _travellerPostsRepository.GetAllPosts();
            return Ok(posts);
        }

        [HttpPost("createTravellerPost")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<TravellerPostDto>> CreateTravellerPost(CreateTravellerPostRequest createTravellerPostRequest)
        {
            var post = await _travellerPostsRepository.CreatePost(createTravellerPostRequest);
            return Ok(post);
        }

        [HttpPut("updateTravellerPost/{postId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<TravellerPostDto>> UpdateTravellerPost(Guid postId,[FromForm] UpdateTravellerPostRequest updateTravellerPostRequest)
        {
            var post = await _travellerPostsRepository.UpdatePost(postId, updateTravellerPostRequest);
            if (post is not null)
                return Ok(post);
            else
                return BadRequest("update fail");
        }

        [HttpDelete("deleteTravellerPost/{postId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteTravellerPost(Guid postId)
        {
            var result = await _travellerPostsRepository.DeletePost(postId);
            return result ? Ok(result) : BadRequest(result);
        }

        [HttpGet("getUserTravellerPosts")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<List<TravellerPost>>> GetUserTravellerPosts()
        {
            var posts = await _travellerPostsRepository.GetUserTravellerPosts();
            return Ok(posts);
        }

    }
}
