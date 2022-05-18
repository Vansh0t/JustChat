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

        ChatMessage CreateChatMessage(int senderId, string senderUsername, string text, DateTime? sendTime=null);
    }
}