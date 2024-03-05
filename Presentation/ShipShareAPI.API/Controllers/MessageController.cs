using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShipShareAPI.Application.Interfaces.Repositories;

namespace ShipShareAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        public MessageController(IMessageRepository messageRepository)
        {
            _messageRepository = Guard.Against.Null(messageRepository);
        }

        [HttpGet("getAllMessages/{recipientId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetAllMessages(Guid recipientId)
        {
            var messages = await _messageRepository.GetAllMessages(recipientId);
            return messages is not null ? Ok(messages) : BadRequest();
        }
    }
}
