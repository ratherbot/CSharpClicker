namespace CSharpClicker.Web.UseCases.GetUserSettings;

public record UserSettingsDto
{
    public string UserName { get; init; }

    public byte[] Avatar { get; init; }

    public string BackgroundPath { get; set; } = "~/images/Backgrounds/default-background.png";
}