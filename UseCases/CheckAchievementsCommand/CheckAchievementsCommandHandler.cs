using CSharpClicker.Web.Infrastructure.Abstractions;
using CSharpClicker.Web.UseCases.CheckAchievements;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class CheckAchievementsCommandHandler : IRequestHandler<CheckAchievementsCommand, Unit>
{
    private readonly IAppDbContext appDbContext;
    private readonly ICurrentUserAccessor currentUserAccessor;

    public CheckAchievementsCommandHandler(IAppDbContext appDbContext, ICurrentUserAccessor currentUserAccessor)
    {
        this.appDbContext = appDbContext;
        this.currentUserAccessor = currentUserAccessor;
    }

    public async Task<Unit> Handle(CheckAchievementsCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserAccessor.GetCurrentUserId();

        var achievements = await appDbContext.Achievements
            .Where(a => !a.IsUnlocked && a.RequiredQuantity <= appDbContext.UserBoosts
                .Where(ub => ub.UserId == userId && ub.BoostId == a.BoostId)
                .Sum(ub => ub.Quantity))
            .ToListAsync(cancellationToken);

        foreach (var achievement in achievements)
        {
            achievement.IsUnlocked = true;
        }

        await appDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}