using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShipShareAPI.Domain.Entities;
using ShipShareAPI.Persistence.Context;

namespace ShipShareAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ShipShareDbContext _shipShareDbContext;

        public ChatController(ShipShareDbContext shipShareDbContext)
        {
            _shipShareDbContext = shipShareDbContext;
        }

        [HttpGet("getAllConversations/{userId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<List<Conversation>>> GetAllConversations(Guid userId)
        {
            var conversations = await _shipShareDbContext.ConversationUser
                .Where(uc => uc.UserId == userId)
                .Select(uc => uc.Conversation)
                .ToListAsync();
            return Ok(conversations);
        }

    }
}
