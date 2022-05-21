using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace JustChat.Models
{
    [Index("SendDateTime")]
    public class ChatMessage
    {
        public int Id {get;set;}
        [Required]
        public DateTime SendDateTime {get;set;}
        public int? SenderId {get;set;}
        public string? SenderUsername {get;set;}
        public ChatUser? Sender {get;set;}
        [Required]
        public string Text {get;set;}
        public IEnumerable<ChatMedia> ChatMedia{get;set;}
    }
}