using System;
using System.Collections.Generic;
using JustAuth.Utils;
using JustChat.Models;
using JustChat.Models.Contexts;
using Microsoft.EntityFrameworkCore;

namespace JustChat.Tests.Fixtures
{
    public class DbFixture
    {
        private const string ConnectionString = "Data Source=JustChat.Tests.Unit.db;Cache=Shared";
        private static readonly object _lock = new();
        private static bool _databaseInitialized;
        public const string USER_USERNAME = "testuser";
        public const string USER_EMAIL = "test@test.com";
        public const string USER_PASSWORD = "testpwd111";
        public DbFixture()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateMockContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                        ChatUser user = new () {
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
                    }
                    _databaseInitialized = true;
                }
            }
        }
        public DbMain CreateMockContext()
            => new (
                new DbContextOptionsBuilder<DbMain>()
                    .UseSqlite(ConnectionString)
                    .Options);
    }
}