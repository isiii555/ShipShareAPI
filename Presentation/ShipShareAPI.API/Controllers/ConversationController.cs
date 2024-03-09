using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShipShareAPI.Application.Dto.Conversation;
using ShipShareAPI.Application.Interfaces.Repositories;
using ShipShareAPI.Domain.Entities;
using ShipShareAPI.Persistence.Context;

namespace ShipShareAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationRepository _conversationRepository;

        public ConversationController(IConversationRepository conversationRepository)
        {
            _conversationRepository = Guard.Against.Null(conversationRepository);
        }

        [HttpGet("getAllConversations")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<List<Conversation>>> GetAllConversations()
        {
            var conversations = await _conversationRepository.GetAllConversations();
            return Ok(conversations);
        }

        [HttpPost("createConversation/{recipientId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<ConversationDto>> CreateConversation(Guid recipientId)
        {
            var conversation = await _conversationRepository.CreateConversation(recipientId);
            return Ok(conversation);
        }

        [HttpDelete("deleteConversation/{conversationId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<ConversationDto>> DeleteConversation(Guid conversationId)
        {
            var result = await _conversationRepository.DeleteConversation(conversationId);
            return result ? Ok(result) : BadRequest(result);
        }

        [HttpGet("getUsernameWithConversationId/{conversationId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<string?>> GetNameWithConversationId(Guid conversationId)
        {
            var result = await _conversationRepository.GetNameWithConversationId(conversationId);
            return result is not null ? Ok(result) : BadRequest("Not found!");
        }

        [HttpGet("getMessagesConversationId/{conversationId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<List<Message>?>> GetMessagesConversationId(Guid conversationId)
        {
            var result = await _conversationRepository.GetMessagesConversationId(conversationId);
            return result is not null ? Ok(result) : BadRequest("Not found!");
        }

        [HttpGet("getConversationId/{recipientId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<ConversationNameIdDto>> GetConversationId(Guid recipientId)
        {
            var result = await _conversationRepository.GetConversationId(recipientId);
            return result is not null ? Ok(result) : BadRequest("Conversation not found") ;
        }

    }
}
