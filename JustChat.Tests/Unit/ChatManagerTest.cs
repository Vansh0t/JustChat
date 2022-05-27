using System;
using System.Linq;
using JustChat.Models.Contexts;
using JustChat.Services.Chat;
using JustChat.Tests.Fixtures;
using Xunit;

namespace JustChat.Tests.Unit;

public class ChatManagerTest:IClassFixture<DbFixture>
{
    private readonly IChatManager _chatManager;
    private readonly DbMain _context;
    public ChatManagerTest(DbFixture fixture) {
        _context = fixture.CreateMockContext();
        _chatManager = new ChatManager(_context);
    }
    [Fact]
    public void TestCreateChatMessage()
    {
        var msg = _chatManager.CreateChatMessage(1, "testuser", "some text", null);
        Assert.NotNull(msg);
        Assert.Equal(DateTime.UtcNow.Second, msg.SendDateTime.Second);
        msg = _chatManager.CreateChatMessage(1, "testuser", "some text", DateTime.UtcNow.AddSeconds(3));
        Assert.NotNull(msg);
        Assert.Equal(DateTime.UtcNow.Second+3, msg.SendDateTime.Second);
    }
    [Fact]
    public void TestGetBeforeTimestampQueryAsc()
    {
        var query = _chatManager.GetBeforeTimestampQueryAsc(DateTime.UtcNow.AddMinutes(100));
        var arr = query.ToArray();
        Assert.Equal(25, arr.Length); // 25 is limit per batch in default ChatManager
        query = _chatManager.GetBeforeTimestampQueryAsc(DateTime.UtcNow.AddMinutes(22));
        arr = query.ToArray();
        Assert.Equal(23, arr.Length);
        var first = arr[0];
        var last = arr[^1];
        Assert.True(last.SendDateTime>first.SendDateTime);
    }
    [Fact]
    public void TestGetBeforeTimestampQueryDesc()
    {
        var query = _chatManager.GetBeforeTimestampQueryDesc(DateTime.UtcNow.AddMinutes(100));
        var arr = query.ToArray();
        Assert.Equal(25, arr.Length); // 25 is limit per batch in default ChatManager
        query = _chatManager.GetBeforeTimestampQueryDesc(DateTime.UtcNow.AddMinutes(22));
        arr = query.ToArray();
        Assert.Equal(23, arr.Length);
        var first = arr[0];
        var last = arr[^1];
        Assert.True(last.SendDateTime<first.SendDateTime);
    }
}