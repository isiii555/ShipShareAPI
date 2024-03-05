using Ardalis.GuardClauses;
using Mapster;
using Microsoft.EntityFrameworkCore;
using ShipShareAPI.Application.Dto.Message;
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
    public class MessageRepository : IMessageRepository
    {
        private readonly ShipShareDbContext _shipShareDbContext;
        private readonly IRequestUserProvider _requestUserProvider;

        public MessageRepository(ShipShareDbContext shipShareDbContext, IRequestUserProvider requestUserProvider)
        {
            _shipShareDbContext = Guard.Against.Null(shipShareDbContext);
            _requestUserProvider = Guard.Against.Null(requestUserProvider);
        }

        public async Task<MessageDto> CreateMessage(Guid conversationId,SendMessageViewModel sendMessageViewModel)
        {
            var message = sendMessageViewModel.Adapt<Message>();
            message.SenderId = _requestUserProvider.GetUserInfo()!.Id;
            message.ConversationId = conversationId;
            var newMessage = (await _shipShareDbContext.Messages.AddAsync(message)).Entity.Adapt<MessageDto>();
            await _shipShareDbContext.SaveChangesAsync();
            return newMessage;
        }

        public async Task<bool> DeleteMessage(Guid recipientId, Guid messageId)
        {
            var user = _requestUserProvider.GetUserInfo();
            var message = await _shipShareDbContext.Messages.FirstOrDefaultAsync(m => m.Id == messageId);
            if (message is not null)
            {
                if (message.RecipientId == recipientId || message.SenderId == user!.Id || user.Role == "Admin") {
                    _shipShareDbContext.Messages.Remove(message);
                    await _shipShareDbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<List<Message>> GetAllMessages(Guid recipientId)
        {
            var user = _requestUserProvider.GetUserInfo();
            var messages = await _shipShareDbContext.Messages
                .Where(m => m.SenderId == user!.Id && m.RecipientId == recipientId)
                .ToListAsync();
            return messages;
        }

        public async Task<MessageDto> UpdateMessage(Guid messageId)
        {
            var message = await _shipShareDbContext.Messages
                .FirstOrDefaultAsync(m => m.Id == messageId);
            if (message is not null)
            {
                message.IsRead = true;
                var messageDto = _shipShareDbContext.Messages.Update(message).Entity.Adapt<MessageDto>();
                await _shipShareDbContext.SaveChangesAsync();
                return messageDto;
            }
            return null;
        }
    }
}
