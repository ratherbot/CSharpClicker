using CSharpClicker.Web.Infrastructure.Abstractions;
using CSharpClicker.Web.Infrastructure.DataAccess;
using CSharpClicker.Web.Infrastructure.Implementations;
using CSharpClicker.Web.UseCases.BuyBoost;
using CSharpClicker.Web.UseCases.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CSharpClicker.Web.Controllers;

[Route("boost")]
public class BoostController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IAppDbContext appDbContext;
    private readonly ICurrentUserAccessor currentUserAccessor;

    public BoostController(IMediator mediator, IAppDbContext appDbContext, ICurrentUserAccessor currentUserAccessor)
    {
        this.mediator = mediator;
        this.appDbContext = appDbContext;
        this.currentUserAccessor = currentUserAccessor;
    }

    [HttpPost("buy")]
    public async Task<ScoreBoostDto> Buy(BuyBoostCommand command)
        => await mediator.Send(command);

    [HttpPost("check-achievements")]
    public async Task<IActionResult> CheckAchievements()
    {
        var userId = currentUserAccessor.GetCurrentUserId();

        var achievements = await appDbContext.Achievements
            .Where(a => !a.IsUnlocked && a.RequiredQuantity <= appDbContext.UserBoosts
                .Count(ub => ub.BoostId == a.BoostId && ub.UserId == userId))
            .ToListAsync();

        foreach (var achievement in achievements)
        {
            achievement.IsUnlocked = true;
        }

        await appDbContext.SaveChangesAsync();

        return Ok(new { message = "Достижения обновлены." });
    }
}