using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using JustChat.Models;
using Microsoft.AspNetCore.Authorization;
using JustAuth.Services.Auth;
using JustAuth.Controllers;

namespace JustChat.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly ILogger<ProfileController> _logger;
    private readonly IUserManager<ChatUser> _userManager;

    public ProfileController(ILogger<ProfileController> logger,
                            IUserManager<ChatUser> userManager
    )
    {
        _userManager = userManager;
        _logger = logger;
    }
    
    public async Task<IActionResult> Index()
    {
        var userId = User.GetUserId();
        var result = await _userManager.GetUserAsync(userId);
        if(result.IsError)
            return result.ToActionResult();
        ViewData["User"] = result.ResultObject;
        return View();
    }
}
