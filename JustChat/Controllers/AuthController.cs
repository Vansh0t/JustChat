﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using JustChat.Models;

namespace JustChat.Controllers;

public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }

    public IActionResult PasswordReset(string rst)
    {
        if(rst is null || rst.Trim() == "") return BadRequest();
        ViewData["token"] = rst;
        return View();
    }

    public IActionResult EmailChange()
    {
        return View();
    }
    public IActionResult EmailConfirm(string vrft)
    {
        if(vrft is null || vrft.Trim() == "") return BadRequest();
        ViewData["token"] = vrft;
        return View();
    }
    public IActionResult SignIn()
    {
        return View();
    }

    public IActionResult SignUp()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
