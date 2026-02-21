using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UI_Test.Models;
using UI_Test.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDataProtection();
builder.Services.AddSingleton<IUrlShortenerService, UrlShortenerService>();
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddSingleton<IEncryptedConfigurationService, EncryptedConfigurationService>();

var appConfig = builder.Configuration.GetSection("AppSecurity").Get<AppSecurityOptions>() ?? new AppSecurityOptions();
var tokenOptions = builder.Configuration.GetSection("Token").Get<TokenOptions>() ?? new TokenOptions();

builder.Services.Configure<AppSecurityOptions>(builder.Configuration.GetSection("AppSecurity"));
builder.Services.Configure<TokenOptions>(builder.Configuration.GetSection("Token"));

var tempProvider = builder.Services.BuildServiceProvider();
var encryptedConfigurationService = tempProvider.GetRequiredService<IEncryptedConfigurationService>();
var decryptedSigningKey = encryptedConfigurationService.TryDecrypt(tokenOptions.SigningKeyEncrypted) ?? tokenOptions.SigningKeyEncrypted;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(decryptedSigningKey)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = tokenOptions.Issuer,
            ValidAudience = tokenOptions.Audience,
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
