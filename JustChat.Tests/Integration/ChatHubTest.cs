using JustChat.Tests.Fixtures;
using Xunit;
using JustChat.Models.Contexts;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;

namespace JustChat.Tests.Integration
{
    public class ChatHubTest:IClassFixture<ChatAppFactory>
    {
        private readonly ChatAppFactory _app;
        private readonly DbMain _context;
        private readonly HubConnection _conn;
        public ChatHubTest(ChatAppFactory app) {
            _app = app;
            _context = app.GetContext();
            var httpClient = app.CreateClient();
            var builder = new HubConnectionBuilder();
            var jwtCookie = app.GetJwtCookie(DbFixture.USER_USERNAME, DbFixture.USER_PASSWORD);
            builder.WithUrl("https://localhost/chat", opt=> {
                opt.HttpMessageHandlerFactory = _ => _app.Server.CreateHandler();
                opt.Headers.Add("Cookie", jwtCookie);
            });
            _conn = builder.Build();
            _conn.StartAsync().GetAwaiter().GetResult();
        }
        [Fact]
        public async Task TestSendMessageSuccess() {
            const string TEST_MESSAGE_TEXT = "TestSendMessage";
            string recievedText = null, recievedSender = null;
            _conn.On<DateTime, string, string, string>("ReceiveMessage", 
                (time, sender, text, avatarUrl)=>{
                    recievedSender = sender;
                    recievedText = text;
                });
            await _conn.InvokeAsync("SendMessage", TEST_MESSAGE_TEXT);
            await Task.Delay(1000);
            var msgInDb = await _context.ChatMessages.FirstOrDefaultAsync(_=>_.Text == TEST_MESSAGE_TEXT);
            Assert.NotNull(msgInDb);
            Assert.Equal(TEST_MESSAGE_TEXT, recievedText);
            Assert.Equal(ChatAppFactory.USER_USERNAME, recievedSender);
            _conn.Remove("ReceiveMessage");
        }
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task TestSendMessageFail(string messageText) {
            await _conn.InvokeAsync("SendMessage", messageText);
            var msgInDb = await _context.ChatMessages.FirstOrDefaultAsync(_=>_.Text == messageText);
            Assert.Null(msgInDb);
        }
        [Fact]
        public async Task TestInitChat() {
            Dictionary<string, object> user = null;
            List<object> messagesList = new();
            _conn.On<Dictionary<string, object>, List<object>>("InitChat", 
                (userData, messages)=>{
                    user = userData;
                    messagesList = messages;
                });
            await _conn.InvokeAsync("InitChat");
            await Task.Delay(1000);
            Assert.Equal(ChatAppFactory.USER_USERNAME, user["username"].ToString());
            Assert.True(messagesList.Count!=0);
            _conn.Remove("InitChat");
        }
        [Theory]
        [InlineData(-2)]
        [InlineData(2)]
        public async Task TestGetMessages(int hoursOffset) {
            List<object> messagesList = new();
            _conn.On<List<object>>("ReceiveMessages", 
                (messages)=>{
                    messagesList = messages;
                });
            await _conn.InvokeAsync("GetMessages", DateTime.UtcNow.AddHours(hoursOffset));
            await Task.Delay(1000);
            if(hoursOffset==2) 
                Assert.True(messagesList.Count==25);//Max batch size
            else if(hoursOffset==-2)
                Assert.True(messagesList.Count==0);
            _conn.Remove("ReceiveMessages");
        }
    }
}