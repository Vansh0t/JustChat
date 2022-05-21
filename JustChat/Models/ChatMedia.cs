using JustFile.Models;

namespace JustChat.Models
{
    public class ChatMedia :FileMeta
    {
        public int ChatMessageId {get;set;}
        public ChatMessage ChatMessage {get;set;}
    }
}