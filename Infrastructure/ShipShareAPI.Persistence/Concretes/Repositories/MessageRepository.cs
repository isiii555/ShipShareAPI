using ShipShareAPI.Application.Dto.Message;
using ShipShareAPI.Application.Interfaces.Repositories;
using ShipShareAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Persistence.Concretes.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        public Task<MessageDto> CreateMessage(Message message)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteMessage(Guid recipientId, Guid messageId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Message>> GetAllMessages(Guid recipientId)
        {
            throw new NotImplementedException();
        }

        public Task<MessageDto> UpdateMessage(Guid messageId)
        {
            throw new NotImplementedException();
        }
    }
}
