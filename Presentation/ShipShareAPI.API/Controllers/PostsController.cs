﻿using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShipShareAPI.Application.Dto.Post;
using ShipShareAPI.Application.Interfaces.Repositories;

namespace ShipShareAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ISenderPostsRepository _senderPostsRepository;
        public PostsController(ISenderPostsRepository senderPostsRepository)
        {
            _senderPostsRepository = Guard.Against.Null(senderPostsRepository);
        }

        [HttpGet("getAllSenderPosts")]
        public async Task<IActionResult> GetAllSenderPosts()
        {
            return Ok(await _senderPostsRepository.GetAllPosts());
        }

        [HttpPost("createSenderPost")]
        public async Task<IActionResult> CreateSenderPost(CreateSenderPostRequest createSenderPostRequest)
        {
            return Ok(await _senderPostsRepository.CreatePost(createSenderPostRequest));
        }

        [HttpPost("updateSenderPost")]
        public async Task<IActionResult> UpdateSenderPost(Guid postId,UpdateSenderPostRequest updateSenderPostRequest)
        {
            var result = await _senderPostsRepository.UpdatePost(postId, updateSenderPostRequest);
            if (result is not null) 
                return Ok(result);
            else 
                return BadRequest(result);
        }

        [HttpDelete("deleteSenderPost")]
        public async Task<IActionResult> DeleteSenderPost(Guid postId)
        {
            var result = await _senderPostsRepository.DeletePost(postId);
            if (result)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}