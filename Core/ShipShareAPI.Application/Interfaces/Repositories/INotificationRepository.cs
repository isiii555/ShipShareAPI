using ShipShareAPI.Application.Dto.Notification;
using ShipShareAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetAllNotifications();
        Task<bool> UpdateNotification(Guid notificationId);
        Task<bool> DeleteNotification(Guid notificationId);
        Task<NotificationDto> CreateNotification(CreateNotificationRequest request);
    }
}
