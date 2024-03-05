using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipShareAPI.Application.Dto.Post.SenderPost;
using ShipShareAPI.Application.Interfaces.Repositories;

namespace ShipShareAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SenderPostController : ControllerBase
    {
        private readonly ISenderPostRepository _senderPostsRepository;
        public SenderPostController(ISenderPostRepository senderPostsRepository)
        {
            _senderPostsRepository = Guard.Against.Null(senderPostsRepository);
        }

        [HttpGet("getAllSenderPosts")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetAllSenderPosts()
        {
            var posts = await _senderPostsRepository.GetAllPosts();
            return Ok(posts);
        }

        [HttpPost("createSenderPost")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateSenderPost(CreateSenderPostRequest createSenderPostRequest)
        {
            var post = await _senderPostsRepository.CreatePost(createSenderPostRequest);
            return Ok(post);
        }

        [HttpPut("updateSenderPost/{postId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateSenderPost(Guid postId,UpdateSenderPostRequest updateSenderPostRequest)
        {
            var result = await _senderPostsRepository.UpdatePost(postId, updateSenderPostRequest);
            if (result is not null) 
                return Ok(result);
            else 
                return BadRequest(result);
        }

        [HttpDelete("deleteSenderPost/{postId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteSenderPost(Guid postId)
        {
            var result = await _senderPostsRepository.DeletePost(postId);
            if (result)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("getUserSenderPosts")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserSenderPosts()
        {
            var posts = await _senderPostsRepository.GetUserSenderPosts();
            return Ok(posts);
        }
    }
}
