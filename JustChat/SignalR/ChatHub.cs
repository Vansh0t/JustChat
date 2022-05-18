using JustChat.Models.Contexts;
using Microsoft.AspNetCore.SignalR;
using JustAuth.Controllers;
using Microsoft.AspNetCore.Authorization;
using JustAuth.Services.Auth;
using JustChat.Models;
using Microsoft.EntityFrameworkCore;
using JustChat.Services.Chat;

namespace JustChat.SignalR
{
    [Authorize("IsEmailVerified")]
    public class ChatHub:Hub
    {
        
        private readonly DbMain _context;
        private readonly ILogger<ChatHub> _logger;
        private readonly IUserManager<ChatUser> _userManager;
        private readonly IChatManager _chatManager;
        public ChatHub(DbMain context,
                       ILogger<ChatHub> logger, 
                       IUserManager<ChatUser> userManager,
                       IChatManager chatManager
                       ):base() {
            _chatManager = chatManager;
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }
        public async Task SendMessage(string message) {
            if(message is null || message.Trim() == "") return;
            var username = Context.User.GetUserName();
            _chatManager.CreateChatMessage(int.Parse(Context.UserIdentifier), username, message);
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("ReceiveMessage", DateTime.UtcNow, username, message);
        }
        public async Task InitChat() {
                
            var username = Context.User.GetUserName();
            var id = int.Parse(Context.UserIdentifier);
            (var messages, var maxPages) = await _chatManager.GetPaginatedQueryAscAsync(0);
            var prepMessages = await messages.Select(_=>new {
                time = _.SendDateTime,
                sender = _.SenderUsername,
                text = _.Text
            }).ToArrayAsync();
            await Clients.Caller.SendAsync("InitChat",
                                        new {
                                            Id = id,
                                            Username=username
                                        },
                                        prepMessages,
                                        maxPages
                                        );
        }
        public async Task GetMessages(int page) {
            (var messages, var maxPages) = await _chatManager.GetPaginatedQueryAscAsync(page);
            var prepMessages = await messages.Select(_=>new {
                time = _.SendDateTime,
                sender = _.SenderUsername,
                text = _.Text
            }).ToArrayAsync();
            await Clients.Caller.SendAsync("ReceiveMessages", prepMessages, maxPages);
        }

    }
    
}