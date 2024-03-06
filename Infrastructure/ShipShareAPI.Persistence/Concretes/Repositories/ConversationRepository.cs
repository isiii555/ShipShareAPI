using Ardalis.GuardClauses;
using Mapster;
using Microsoft.EntityFrameworkCore;
using ShipShareAPI.Application.Dto.Conversation;
using ShipShareAPI.Application.Interfaces.Auth;
using ShipShareAPI.Application.Interfaces.Providers;
using ShipShareAPI.Application.Interfaces.Repositories;
using ShipShareAPI.Domain.Entities;
using ShipShareAPI.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShipShareAPI.Persistence.Concretes.Repositories
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly ShipShareDbContext _shipShareDbContext;
        private readonly IRequestUserProvider _requestUserProvider;
        private readonly IUserManager _userManager;

        public ConversationRepository(IRequestUserProvider requestUserProvider, ShipShareDbContext shipShareDbContext, IUserManager userManager)
        {
            _requestUserProvider = Guard.Against.Null(requestUserProvider);
            _shipShareDbContext = Guard.Against.Null(shipShareDbContext);
            _userManager = Guard.Against.Null(userManager);
        }

        public async Task<ConversationDto> CreateConversation(Guid recipientId)
        {
            var user = await _userManager.GetUserWithId(_requestUserProvider.GetUserInfo()!.Id);
            var recipientUser = await _userManager.GetUserWithId(recipientId);

            bool conversationExists = _shipShareDbContext.ConversationUser
                        .Where(cu => cu.UserId == user!.Id || cu.UserId == recipientId)
                        .GroupBy(cu => cu.ConversationId)
                        .Any(group => group.Count() == 2);

            if (!conversationExists)
            {
                _shipShareDbContext!.Entry(user!)!.State = EntityState.Unchanged;
                _shipShareDbContext!.Entry(recipientUser!)!.State = EntityState.Unchanged;
                var conversation = new Conversation()
                {
                    Users = new List<User>() { user!, recipientUser! },
                };

                var dto = (await _shipShareDbContext.Conversations.AddAsync(conversation)).Entity.Adapt<ConversationDto>();
                await _shipShareDbContext.SaveChangesAsync();
                return dto;
            }
            throw new Exception("Conversation already exist!");
        }

        public async Task<bool> DeleteConversation(Guid conversationId)
        {
            var user = _requestUserProvider.GetUserInfo();
            var conversation = await _shipShareDbContext.ConversationUser.FirstOrDefaultAsync(c => c.Id == conversationId);
            if (conversation is not null)
            {
                if (conversation.UserId == user!.Id)
                {
                    _shipShareDbContext.ConversationUser.Remove(conversation);
                    await _shipShareDbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<List<Conversation>> GetAllConversations()
        {
            var conversations = await _shipShareDbContext.ConversationUser
                .Where(c => c.UserId == _requestUserProvider.GetUserInfo()!.Id)
                .Include(cu => cu.Conversation)
                .ThenInclude(c => c.Messages)
                .Select(c => c.Conversation)
                .ToListAsync();
            return conversations;
        }
        
        public async Task<string?> GetNameWithConversationId(Guid conversationId)
        {
            var user = _requestUserProvider?.GetUserInfo();
            var conversations = await _shipShareDbContext.ConversationUser.Where(cu => cu.ConversationId == conversationId).ToListAsync();
            foreach (var item in conversations)
            {
                if (item.UserId != user!.Id)
                {
                    var userMain = await _userManager.GetUserWithId(item.UserId);
                    if (userMain is not null)
                    {
                        return userMain.Username;
                    }
                    return null;
                }
            }
            return null;
        }

        public async Task<Guid?> GetRecipientId(Guid conversationId)
        {
            var user = _requestUserProvider?.GetUserInfo();
            var conversations = await _shipShareDbContext.ConversationUser.Where(cu => cu.ConversationId == conversationId).ToListAsync();
            foreach (var item in conversations)
            {
                if (item.UserId != user!.Id)
                {
                    var userMain = await _userManager.GetUserWithId(item.UserId);
                    if (userMain is not null)
                    {
                        return userMain.Id;
                    }
                    return null;
                }
            }
            return null;
        }
    }
}
