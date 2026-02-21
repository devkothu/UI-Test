namespace UI_Test.Models;

public class AppSecurityOptions
{
    public string ApiToken { get; set; } = "local-dev-api-token";
    public int RateLimitPerMinute { get; set; } = 60;
}
