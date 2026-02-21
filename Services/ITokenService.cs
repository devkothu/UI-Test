namespace UI_Test.Services;

public interface ITokenService
{
    string GenerateAccessToken(string subject, IEnumerable<string>? roles = null);
}
