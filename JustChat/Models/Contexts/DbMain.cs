using JustAuth.Data;
using Microsoft.EntityFrameworkCore;

namespace JustChat.Models.Contexts
{
    public class DbMain: AuthDbMain<ChatUser>
    {
        public DbSet<ChatMessage> ChatMessages {get;set;}
        public DbMain(DbContextOptions options) : base(options) {

        }
    }
}