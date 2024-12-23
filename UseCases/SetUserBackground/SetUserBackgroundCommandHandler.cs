using CSharpClicker.Web.Infrastructure.Abstractions;
using CSharpClicker.Web.Infrastructure.DataAccess;
using CSharpClicker.Web.UseCases.SetUserBackground;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace CSharpClicker.Web.UseCases.SetUserBackground;

public class SetUserBackgroundCommandHandler : IRequestHandler<SetUserBackgroundCommand, Unit>
{
    private readonly IAppDbContext appDbContext;
    private readonly ICurrentUserAccessor currentUserAccessor;

    public SetUserBackgroundCommandHandler(IAppDbContext appDbContext, ICurrentUserAccessor currentUserAccessor)
    {
        this.appDbContext = appDbContext;
        this.currentUserAccessor = currentUserAccessor;
    }

    public async Task<Unit> Handle(SetUserBackgroundCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserAccessor.GetCurrentUserId();
        var user = await appDbContext.ApplicationUsers
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
        {
            throw new Exception("Пользователь не найден");
        }

        user.BackgroundPath = request.BackgroundPath;
        await appDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}