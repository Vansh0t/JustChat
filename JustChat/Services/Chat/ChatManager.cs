using JustChat.Models;
using JustChat.Models.Contexts;
namespace JustChat.Services.Chat
{
    public class ChatManager:IChatManager
    {
        private readonly DbMain _context;
        public ChatManager (DbMain context) {
            _context = context;
        }
        private const int MESSAGES_BATCH_SIZE = 25; // pagination
        public IQueryable<ChatMessage> GetBeforeTimestampQueryAsc(DateTime time) {
            var messages =  _context.ChatMessages.
                            OrderBy(_=>_.SendDateTime).
                            Where(_=>_.SendDateTime<time).
                            Take(MESSAGES_BATCH_SIZE);
            return messages;
        }
        public IQueryable<ChatMessage> GetBeforeTimestampQueryDesc(DateTime time) {
            var messages =  _context.ChatMessages.
                            OrderByDescending(_=>_.SendDateTime).
                            Where(_=>_.SendDateTime<time).
                            Take(MESSAGES_BATCH_SIZE);
            return messages;
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