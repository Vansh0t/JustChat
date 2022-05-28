using Microsoft.AspNetCore.Mvc;
using JustChat.Models;
using Microsoft.AspNetCore.Authorization;
using JustAuth.Services.Auth;
using JustAuth.Controllers;
using JustChat.Models.Contexts;
using Microsoft.EntityFrameworkCore;

namespace JustChat.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly ILogger<ProfileController> _logger;
    private readonly IUserManager<ChatUser> _userManager;
    private readonly string webRootPath;
    private readonly DbMain _context;

    public ProfileController(ILogger<ProfileController> logger,
                            DbMain context,
                            IUserManager<ChatUser> userManager,
                            IWebHostEnvironment env
    )
    {
        webRootPath = env.WebRootPath;
        _userManager = userManager;
        _logger = logger;
        _context = context;
    }
    
    public async Task<IActionResult> Index()
    {
        var userId = User.GetUserId();
        var user = await _context.Users.Include(_=>_.Avatar).FirstAsync(_=>_.Id == userId);
        ViewData["User"] = user;
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> UploadAvatar(IFormFile image){
        if(image is null || !Path.HasExtension(image.FileName))
            return BadRequest();
        var userId = User.GetUserId();
        string extension = Path.GetExtension( image.FileName);
        string fileName = $"avatar{extension}";
        string path = Path.Join(webRootPath, "media", "users", userId.ToString());
        string fullPath = Path.Join(path, fileName);
        string url = $"/media/users/{userId}/{fileName}";
        Directory.CreateDirectory(path);
        
        //try delete existing from filesystem
        var existingFiles = Directory.GetFiles(path).Where(_=> Path.GetFileName(_).StartsWith("avatar"));
        foreach(var f in existingFiles)
            System.IO.File.Delete(f);
        //write new file
        using var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
        await image.CopyToAsync(fs);
        //try modifying or creating existing file meta in db
        UserAvatar avatar = await _context.UserAvatars.FirstOrDefaultAsync(_=>_.ChatUserId==userId);
        if(avatar is null) {
            avatar = new()
            {
                Name = fileName,
                Path = path,
                Url = url,
                ChatUserId = userId
            };
            _context.UserAvatars.Add(avatar);
        }
        else {
            avatar.Path = path;
            avatar.Name = fileName;
            avatar.Url = url;
        }
        await _context.SaveChangesAsync();
        return Redirect("/Profile");
    }
}
