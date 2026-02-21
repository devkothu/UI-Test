namespace UI_Test.Models;

public class ShortUrlEntry
{
    public string Code { get; set; } = string.Empty;
    public string OriginalUrl { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public int VisitCount { get; set; }
}
