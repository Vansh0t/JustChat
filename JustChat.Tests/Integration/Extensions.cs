using System;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;

namespace JustChat.Tests.Integration
{
    public static class Extensions
    {
        public static string GetJwtCookie(this WebApplicationFactory<ProgramEntry> app, string username, string password)
         {
            var client = app.CreateClient();
            var content = Utlis.MakeStringContent("credential", username, "password", password);
            var result = client.PostAsync("/auth/signin", content).Result;
            if((int)result.StatusCode != 200)
                throw new Exception("SignIn request returned unsuccessful status code");
            result.Headers.TryGetValues("Set-Cookie", out var cookies);
            return cookies.Single(_=>_.StartsWith("jwt="));
        }
    }
}