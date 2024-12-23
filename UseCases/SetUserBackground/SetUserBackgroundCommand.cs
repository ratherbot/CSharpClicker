using MediatR;

namespace CSharpClicker.Web.UseCases.SetUserBackground;

public class SetUserBackgroundCommand : IRequest<Unit>
{
    public string BackgroundPath { get; set; } = string.Empty;
}