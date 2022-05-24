using JustChat.Models;

namespace JustChat.Services.Chat
{
    public interface IChatManager
    {
        /// <summary>
        /// Get query of all messages posted before DateTime, ordered ascending
        /// </summary>
        /// <param name="timestamp">Milliseconds since Epoch</param>
        IQueryable<ChatMessage> GetBeforeTimestampQueryAsc(DateTime time);
        /// <summary>
        /// Get query of all messages posted before DateTime, ordered descending
        /// </summary>
        /// <param name="timestamp">Milliseconds since Epoch</param>
        IQueryable<ChatMessage> GetBeforeTimestampQueryDesc(DateTime time);
        /// <summary>
        /// Creates new ChatMessage with given parameters. If sendTime=null, DateTime.UtcNow is used.
        /// Adds new message to db context, but doesn't save it.
        /// </summary>
        ChatMessage CreateChatMessage(int senderId, string senderUsername, string text, DateTime? sendTime=null);
    }
}