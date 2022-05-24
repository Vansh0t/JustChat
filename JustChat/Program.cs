using JustAuth;
using JustChat.Models;
using JustChat.Models.Contexts;
using Microsoft.EntityFrameworkCore;
using JustAuth.Data;
using JustChat.SignalR;
using JustChat.Services.Chat;
using Microsoft.Extensions.Logging.Console;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
// Add services to the container.
services.AddControllersWithViews();
services.AddDbContext<IAuthDbMain<ChatUser>, DbMain>(options=>{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
services.AddJustAuth<ChatUser>(options => {
    options.UseEmailConfirmRedirect("/Auth/EmailConfirm");
    options.UsePasswordResetRedirect("/Auth/PasswordReset");
});
services.AddScoped<IChatManager, ChatManager>();
services.AddSignalR();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(options=> {
    options.MapDefaultControllerRoute();
    //options.MapControllers();
});
app.MapHub<ChatHub>("/chat");
app.Run();
