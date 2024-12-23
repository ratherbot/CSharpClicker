namespace CSharpClicker.Web.Domain;

public class Achievement
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public byte[] Icon { get; set; } = Array.Empty<byte>();
    public bool IsUnlocked { get; set; } = false;
    public int BoostId { get; set; }
    public Boost Boost { get; set; } = null!; 
    public int RequiredQuantity { get; set; }
}