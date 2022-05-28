using JustChat.Models.Contexts;
using Microsoft.AspNetCore.SignalR;
using JustAuth.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using JustChat.Services.Chat;

namespace JustChat.SignalR
{
    [Authorize("IsEmailVerified")]
    public class ChatHub:Hub
    {
        
        private readonly DbMain _context;
        private readonly ILogger<ChatHub> _logger;
        private readonly IChatManager _chatManager;
        public ChatHub(DbMain context,
                       ILogger<ChatHub> logger, 
                       IChatManager chatManager
                       ):base() {
            _chatManager = chatManager;
            _context = context;
            _logger = logger;
        }
        public async Task SendMessage(string message) {
            if(message is null || message.Trim() == "") return;
            var id = int.Parse(Context.UserIdentifier);
            var username = Context.User.GetUserName();
            _chatManager.CreateChatMessage(int.Parse(Context.UserIdentifier), username, message);
            var avatar = await _context.UserAvatars.FirstOrDefaultAsync(_=>_.ChatUserId==id);
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("ReceiveMessage", DateTime.UtcNow, username, message, avatar?.Url);
        }
        public async Task InitChat() {
                
            var username = Context.User.GetUserName();
            var id = int.Parse(Context.UserIdentifier);
            var messages = _chatManager.GetBeforeTimestampQueryDesc(DateTime.UtcNow);
            messages = messages.OrderBy(_=>_.SendDateTime);
            var prepMessages = await messages.Select(_=>new {
                time = _.SendDateTime,
                sender = _.SenderUsername,
                text = _.Text,
                avatarUrl = _.Sender.Avatar.Url
            }).ToArrayAsync();
            await Clients.Caller.SendAsync("InitChat",
                                        new {
                                            Id = id,
                                            Username=username
                                        },
                                        prepMessages
                                        );
        }
        public async Task GetMessages(DateTime time) {
            var messages = _chatManager.GetBeforeTimestampQueryDesc(time);
            var prepMessages = await messages.Select(_=>new {
                time = _.SendDateTime,
                sender = _.SenderUsername,
                text = _.Text,
                avatarUrl = _.Sender.Avatar.Url
            }).ToArrayAsync();
            await Clients.Caller.SendAsync("ReceiveMessages", prepMessages);
        }

    }
    
}