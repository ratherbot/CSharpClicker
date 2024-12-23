﻿using CSharpClicker.Web.UseCases.GetLeaderboard;
using CSharpClicker.Web.UseCases.GetUserSettings;
using CSharpClicker.Web.UseCases.SetUserAvatar;
using CSharpClicker.Web.UseCases.SetUserBackground;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSharpClicker.Web.Controllers;

[Route("user")]
[Authorize]
public class UserController : Controller
{
    private readonly IMediator mediator;

    public UserController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("avatar")]
    public async Task<IActionResult> SetAvatar(SetUserAvatarCommand command)
    {
        await mediator.Send(command);

        return RedirectToAction("Settings", "User");
    }

    [HttpGet("leaderboard")]
    public async Task<IActionResult> Leaderboard(GetLeaderboardQuery query)
    {
        var leaderboard = await mediator.Send(query);

        return View(leaderboard);
    }

    [HttpGet("settings")]
    public async Task<IActionResult> Settings()
    {
        var userSettings = await mediator.Send(new GetCurrentUserSettingsQuery());

        ViewData["BackgroundPath"] = userSettings.BackgroundPath;

        return View(userSettings);
    }

    [HttpPost("background")]
    public async Task<IActionResult> SetBackground([FromBody] SetUserBackgroundCommand command)
    {
        await mediator.Send(command);

        return Ok(new { message = "Фон сохранён успешно." });
    }
}