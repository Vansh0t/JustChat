using JustFile.Models;

namespace JustChat.Models
{
    public class UserAvatar : FileMeta
    {
        public int ChatUserId {get;set;}
        public ChatUser ChatUser {get;set;}
    }
}