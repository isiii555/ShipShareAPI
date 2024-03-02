using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShipShareAPI.Application.Interfaces.Repositories;

namespace ShipShareAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationController(INotificationRepository notificationRepository)
        {
            _notificationRepository = Guard.Against.Null(notificationRepository);
        }

        [HttpPost("updateNotification/{notificationId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<bool>> UpdateNotification(Guid notificationId)
        {
            var result = await _notificationRepository.UpdateNotification(notificationId);
            return result ? Ok(result) : BadRequest();
        }

        [HttpDelete("deleteNotification/{notificationId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<bool>> DeleteNotification(Guid notificationId)
        {
            var result = await _notificationRepository.DeleteNotification(notificationId);
            return result ? Ok(result) : BadRequest();
        }

        [HttpGet("getAllNotifications")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<bool>> GetAllNotifications()
        {
            var result = await _notificationRepository.GetAllNotifications();
            return Ok(result);
        }
    }
}
