using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UI_Test.Models;

namespace UI_Test.Services;

public class TokenService(IOptions<TokenOptions> options, IEncryptedConfigurationService encryptedConfigurationService) : ITokenService
{
    private readonly TokenOptions _tokenOptions = options.Value;
    private readonly IEncryptedConfigurationService _encryptedConfigurationService = encryptedConfigurationService;

    public string GenerateAccessToken(string subject, IEnumerable<string>? roles = null)
    {
        var signingKey = _encryptedConfigurationService.TryDecrypt(_tokenOptions.SigningKeyEncrypted)
            ?? _tokenOptions.SigningKeyEncrypted;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, subject),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        if (roles != null)
        {
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        }

        var token = new JwtSecurityToken(
            issuer: _tokenOptions.Issuer,
            audience: _tokenOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_tokenOptions.ExpirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
