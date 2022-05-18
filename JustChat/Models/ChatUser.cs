using JustAuth.Data;
namespace JustChat.Models
{
    public class ChatUser:AppUser
    {
        public IEnumerable<ChatMessage> Messages {get;set;}
    }
}