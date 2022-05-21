using JustChat.Models;

namespace JustChat.Services.Chat
{
    public interface IChatManager
    {
        /// <summary>
        /// Get ascend ordered by datetime query with page number applied
        /// </summary>
        /// <param name="GetPaginatedQueryAscAsync"></param>
        /// <returns>Tuple with <see cref="IQueryable"/> with paginated messages and number of the last page</returns>
        Task<(IQueryable<ChatMessage>, double)> GetPaginatedQueryAscAsync(int page);
        /// <summary>
        /// Get descend ordered by datetime query with page number applied
        /// </summary>
        /// <param name="GetPaginatedQueryAscAsync"></param>
        /// <returns>Tuple with <see cref="IQueryable"/> with paginated messages and number of the last page</returns>
        Task<(IQueryable<ChatMessage>, double)> GetPaginatedQueryDescAsync(int page);
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
        ChatMessage CreateChatMessage(int senderId, string senderUsername, string text, DateTime? sendTime=null);
    }
}