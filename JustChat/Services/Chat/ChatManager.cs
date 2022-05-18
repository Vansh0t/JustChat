using JustChat.Models;
using JustChat.Models.Contexts;
using Microsoft.EntityFrameworkCore;
namespace JustChat.Services.Chat
{
    public class ChatManager:IChatManager
    {
        private readonly ILogger<ChatManager> _logger;
        private readonly DbMain _context;
        public ChatManager (ILogger<ChatManager> logger, DbMain context) {
            _logger = logger;
            _context = context;
        }
        private const int MESSAGES_BATCH_SIZE = 25; // pagination
        public async Task<(IQueryable<ChatMessage>, double)> GetPaginatedQueryAscAsync(int page) {
            double maxPages = Math.Floor((double)(await _context.ChatMessages.CountAsync() / MESSAGES_BATCH_SIZE));
            if(page>maxPages) return (null, maxPages);
            var messages =  _context.ChatMessages.
                            OrderBy(_=>_.SendDateTime).
                            Skip(MESSAGES_BATCH_SIZE * page).
                            Take(MESSAGES_BATCH_SIZE);
            return (messages, maxPages);
        }
        public async Task<(IQueryable<ChatMessage>, double)> GetPaginatedQueryDescAsync(int page) {
            double maxPages = Math.Floor((double)(await _context.ChatMessages.CountAsync() / MESSAGES_BATCH_SIZE));
            if(page>maxPages) return (null, maxPages);
            var messages =  _context.ChatMessages.
                            OrderByDescending(_=>_.SendDateTime).
                            Skip(MESSAGES_BATCH_SIZE * page).
                            Take(MESSAGES_BATCH_SIZE);
            return (messages, maxPages);
        }
        public ChatMessage CreateChatMessage(int senderId, string senderUsername, string text, DateTime? sendTime=null) {
            ChatMessage message = new() {
                SenderId = senderId,
                SenderUsername = senderUsername,
                SendDateTime = (DateTime)(sendTime is null? DateTime.UtcNow:sendTime),
                Text = text
            };
            _context.ChatMessages.Add(message);
            return message;
        }
    }
}