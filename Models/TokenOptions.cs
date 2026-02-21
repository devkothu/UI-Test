namespace UI_Test.Models;

public class TokenOptions
{
    public string Issuer { get; set; } = "shorturl-app";
    public string Audience { get; set; } = "shorturl-clients";
    public string SigningKeyEncrypted { get; set; } = "local-dev-signing-key-change-me-now";
    public int ExpirationMinutes { get; set; } = 30;
}
