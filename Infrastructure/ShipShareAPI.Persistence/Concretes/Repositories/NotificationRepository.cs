using Ardalis.GuardClauses;
using Mapster;
using Microsoft.EntityFrameworkCore;
using ShipShareAPI.Application.Dto.Notification;
using ShipShareAPI.Application.Interfaces.Providers;
using ShipShareAPI.Application.Interfaces.Repositories;
using ShipShareAPI.Domain.Entities;
using ShipShareAPI.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Persistence.Concretes.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ShipShareDbContext _shipShareDbContext;
        private readonly IRequestUserProvider _requestUserProvider;
        public NotificationRepository(ShipShareDbContext shipShareDbContext, IRequestUserProvider requestUserProvider)
        {
            _shipShareDbContext = Guard.Against.Null(shipShareDbContext);
            _requestUserProvider = Guard.Against.Null(requestUserProvider);
        }

        public async Task<NotificationDto> CreateNotification(CreateNotificationRequest request)
        {
            var notification = request.Adapt<Notification>();
            var newNotification = (await _shipShareDbContext.Notifications.AddAsync(notification)).Entity;
            await _shipShareDbContext.SaveChangesAsync();
            var newDto = newNotification.Adapt<NotificationDto>();
            return newDto;
        }

        public async Task<bool> DeleteNotification(Guid notificationId)
        {
            var user = _requestUserProvider.GetUserInfo();
            var notification = await _shipShareDbContext.Notifications.FirstOrDefaultAsync(n => n.Id ==  notificationId);
            if (notification is not null)
            {
                if (user!.Id == notification.UserId || user.Role == "Admin")
                {
                    _shipShareDbContext.Notifications.Remove(notification);
                    await _shipShareDbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            else
                return false;
        }

        public async Task<List<Notification>> GetAllNotifications()
        {
            return await _shipShareDbContext.Notifications.ToListAsync();
        }

        public async Task<bool> UpdateNotification(Guid notificationId)
        {
            var user = _requestUserProvider.GetUserInfo();
            var notification = await _shipShareDbContext.Notifications.FirstOrDefaultAsync(n => n.Id == notificationId);
            if (notification is not null)
            {
                if (user!.Id == notification.UserId || user.Role == "Admin")
                {
                    notification.IsRead = true;
                    _shipShareDbContext.Notifications.Update(notification);
                    await _shipShareDbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            else
                return false;
        }
    }
}
