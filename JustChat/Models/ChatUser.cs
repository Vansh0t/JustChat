using JustAuth.Data;

namespace JustChat.Models
{
    public class ChatUser:AppUser
    {
        public UserAvatar Avatar {get;set;} 
        public IEnumerable<ChatMessage> Messages {get;set;}
    }
}