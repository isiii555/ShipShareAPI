using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ShipShareAPI.Application.Dto.Message;
using ShipShareAPI.Application.Interfaces.Auth;
using ShipShareAPI.Application.Interfaces.Repositories;

namespace ShipShareAPI.API.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserManager _userManager;
        private readonly IConversationRepository _conversationRepository;

        public ChatHub(IMessageRepository messageRepository, IUserManager userManager, IConversationRepository conversationRepository)
        {
            _messageRepository = Guard.Against.Null(messageRepository);
            _userManager = Guard.Against.Null(userManager);
            _conversationRepository = Guard.Against.Null(conversationRepository);
        }
        public override async Task OnConnectedAsync()
        {
            await _userManager.UpdateConnectionId(Context.ConnectionId);
        }
        public async Task SendMessageAsync(string conversationId, string recipientId, string text)
        {

            if (recipientId is null)
            {
                var id = await _conversationRepository.GetRecipientId(Guid.Parse(conversationId));
                if (id is not null)
                {
                    recipientId = id.ToString()!;
                }
            }
            var message = new SendMessageViewModel()
            {
                RecipientId = Guid.Parse(recipientId),
                Text = text,
            };
            await _messageRepository.CreateMessage(Guid.Parse(conversationId), message);
            var user = await _userManager.GetUserWithId(Guid.Parse(recipientId));
            if (user is not null)
            {
                await Clients.Client(user!.ConnectionId!).SendAsync("ReceiveMessage",message,conversationId);
                await Console.Out.WriteLineAsync(text);
            }
        }

        public async Task GetId()
        {
            await Clients.Caller.SendAsync("getId", Context.UserIdentifier);
        }
    }
}
