using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using JustAuth.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using JustAuth.Utils;
using Microsoft.AspNetCore.Builder;
using System;
using System.Threading.Tasks;
using JustChat.Models;
using JustChat.Models.Contexts;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Linq;

namespace JustChat.Tests.Fixtures
{
    public class ChatAppFactory:WebApplicationFactory<ProgramEntry>
    {
        
        private const string ConnectionString = "Data Source=JustChat.Tests.Integration.db;Cache=Shared";
        private static bool isDbInitialized;
        public const string USER_USERNAME = "testuser";
        public const string USER_EMAIL = "test@test.com";
        public const string USER_PASSWORD = "testpwd111";
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var connectionString = ConnectionString;
                services.RemoveAll<IAuthDbMain<ChatUser>>();
                services.AddDbContext<IAuthDbMain<ChatUser>, DbMain>(opt=> {
                    opt.UseSqlite(connectionString);
                });
                services.AddHttpsRedirection(opt=>{
                    opt.HttpsPort = 443;
                });
                InitDatabase(services);
            });
        }
        private static void InitDatabase(IServiceCollection services) {
            if (isDbInitialized) return;
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope() ;
            using var context = scope.ServiceProvider.GetRequiredService<DbMain>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var user = context.Users.FirstOrDefault(_=>_.Email==USER_EMAIL);
            user = new () {
                Email = USER_EMAIL,
                Username = USER_USERNAME,
                PasswordHash = Cryptography.HashPassword(USER_PASSWORD),
                IsEmailVerified = true
            };
            context.Users.Add(user);
            List<ChatMessage> messages = new();
            for(int i = 0; i < 100; i++) {
                var msg = new ChatMessage {
                    SendDateTime = DateTime.UtcNow.AddMinutes(i),
                    Sender = user,
                    Text = "Some Text " + i
                };
                messages.Add(msg);
            }
            
            context.ChatMessages.AddRange(messages);
            context.SaveChanges();
            isDbInitialized = true;
        }

        public async Task UsingContext(Func<DbMain, Task> action) {
            using var scope = Services.CreateScope() ;
            using var context = scope.ServiceProvider.GetRequiredService<DbMain>();
            await action(context);
        }
        public DbMain GetContext() {
            var scope = Services.CreateScope() ;
            return scope.ServiceProvider.GetRequiredService<DbMain>();
        }
    }
}