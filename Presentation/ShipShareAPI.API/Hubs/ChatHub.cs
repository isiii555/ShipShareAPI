using Ardalis.GuardClauses;
using Microsoft.AspNetCore.SignalR;
using ShipShareAPI.Application.Dto.Message;
using ShipShareAPI.Application.Interfaces.Auth;
using ShipShareAPI.Application.Interfaces.Providers;
using ShipShareAPI.Application.Interfaces.Repositories;
using System.Security.Claims;

namespace ShipShareAPI.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserManager _userManager;
        private readonly IRequestUserProvider _requestUserProvider;
        private readonly HttpContext? _httpContext;

        public ChatHub(IHttpContextAccessor contextAccessor,IMessageRepository messageRepository, IUserManager userManager, IRequestUserProvider requestUserProvider)
        {
            _messageRepository = Guard.Against.Null(messageRepository);
            _userManager = Guard.Against.Null(userManager);
            _requestUserProvider = Guard.Against.Null(requestUserProvider);
            _httpContext = contextAccessor.HttpContext;
        }
        public override async Task OnConnectedAsync()
        {
            await Console.Out.WriteLineAsync(Context.User.FindFirstValue(ClaimTypes.NameIdentifier.ToString()));
            await _userManager.UpdateConnectionId(Context.ConnectionId);
        }
        public async Task SendMessageAsync(string conversationId,string recipientId,string text)
        {
            if (_httpContext.User.Identity!.IsAuthenticated)
            {
                var message = new SendMessageViewModel()
                {
                    RecipientId = Guid.Parse(recipientId),
                    Text = text,
                };
                await _messageRepository.CreateMessage(Guid.Parse(conversationId), message);
                var user = await _userManager.GetUserWithId(Guid.Parse(recipientId));
                await Clients.Client(user!.ConnectionId!).SendAsync("ReceiveMessage", message);
                await Console.Out.WriteLineAsync(text);
            }
            throw new Exception("user not authenticated!");
        }

        public async Task GetId()
        {
            var userInfo = _requestUserProvider.GetUserInfo();
            await Clients.Caller.SendAsync("getId",userInfo!.Id.ToString());
        }
    }
}
