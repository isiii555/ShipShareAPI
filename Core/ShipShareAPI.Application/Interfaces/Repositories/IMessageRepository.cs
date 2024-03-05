using ShipShareAPI.Application.Dto.Message;
using ShipShareAPI.Domain.Entities;

namespace ShipShareAPI.Application.Interfaces.Repositories
{
    public interface IMessageRepository
    {
        Task<List<Message>> GetAllMessages(Guid recipientId);
        Task<MessageDto> CreateMessage(Guid conversationId,SendMessageViewModel sendMessageViewModel);
        Task<bool> DeleteMessage(Guid recipientId,Guid messageId);
        Task<MessageDto> UpdateMessage(Guid messageId);
    }
}
