using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI_Test.Models;
using UI_Test.Services;

namespace UI_Test.Controllers.Api;

[ApiController]
[Route("api")]
public class ShortUrlApiController(IUrlShortenerService shortenerService, ITokenService tokenService, IConfiguration configuration) : ControllerBase
{
    private readonly IUrlShortenerService _shortenerService = shortenerService;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IConfiguration _configuration = configuration;

    [HttpPost("token")]
    public IActionResult Token([FromHeader(Name = "X-API-KEY")] string? apiKey)
    {
        var configuredApiToken = _configuration["AppSecurity:ApiToken"];
        if (string.IsNullOrWhiteSpace(apiKey) || apiKey != configuredApiToken)
        {
            return Unauthorized(new { message = "Invalid API key." });
        }

        var token = _tokenService.GenerateAccessToken("shorturl-client", new[] { "shorturl.write" });
        return Ok(new { access_token = token, token_type = "Bearer" });
    }

    [Authorize]
    [HttpPost("shorten")]
    public IActionResult Shorten([FromBody] CreateShortUrlRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var entry = _shortenerService.Create(request.Url);
        var baseUrl = $"{Request.Scheme}://{Request.Host}";

        return Ok(new ShortUrlResponse
        {
            Code = entry.Code,
            OriginalUrl = entry.OriginalUrl,
            CreatedAtUtc = entry.CreatedAtUtc,
            ShortUrl = $"{baseUrl}/s/{entry.Code}"
        });
    }

    [Authorize]
    [HttpGet("urls")]
    public IActionResult Urls()
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var response = _shortenerService.GetAll().Select(entry => new ShortUrlResponse
        {
            Code = entry.Code,
            OriginalUrl = entry.OriginalUrl,
            CreatedAtUtc = entry.CreatedAtUtc,
            ShortUrl = $"{baseUrl}/s/{entry.Code}"
        });

        return Ok(response);
    }
}
