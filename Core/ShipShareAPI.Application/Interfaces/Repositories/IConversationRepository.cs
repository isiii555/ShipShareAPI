using ShipShareAPI.Application.Dto.Conversation;
using ShipShareAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Interfaces.Repositories
{
    public interface IConversationRepository
    {
        Task<ConversationDto> CreateConversation(Guid recipientId);
        Task<bool> DeleteConversation(Guid conversationId);
        Task<List<Conversation>> GetAllConversations();
        Task<string?> GetNameWithConversationId(Guid conversationId);
        Task<Guid?> GetRecipientId(Guid conversationId);
        Task<List<Message>> GetMessagesConversationId(Guid conversationId);
    }
}
