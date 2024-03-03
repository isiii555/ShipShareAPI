using ShipShareAPI.Application.Dto.Message;
using ShipShareAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Interfaces.Repositories
{
    public interface IMessageRepository
    {
        Task<List<Message>> GetAllMessages(Guid recipientId);
        Task<MessageDto> CreateMessage(Message message);
        Task<bool> DeleteMessage(Guid recipientId,Guid messageId);
        Task<MessageDto> UpdateMessage(Guid messageId);
    }
}
