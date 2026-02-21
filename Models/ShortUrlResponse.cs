namespace UI_Test.Models;

public class ShortUrlResponse
{
    public string Code { get; set; } = string.Empty;
    public string OriginalUrl { get; set; } = string.Empty;
    public string ShortUrl { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
}
