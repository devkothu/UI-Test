using Microsoft.EntityFrameworkCore;
using UI_Test.Data;
using UI_Test.Models;

namespace UI_Test.Services;

public class UrlShortenerService(AppDbContext dbContext) : IUrlShortenerService
{
    private readonly AppDbContext _dbContext = dbContext;

    public ShortUrlEntry Create(string url)
    {
        var code = GenerateUniqueCode();
        var entry = new ShortUrlEntry
        {
            Code = code,
            OriginalUrl = url,
            CreatedAtUtc = DateTime.UtcNow,
            VisitCount = 0
        };

        _dbContext.ShortUrls.Add(entry);
        _dbContext.SaveChanges();

        return entry;
    }

    public ShortUrlEntry? GetByCode(string code)
    {
        var entry = _dbContext.ShortUrls.SingleOrDefault(x => x.Code == code);
        if (entry is null)
        {
            return null;
        }

        entry.VisitCount++;
        _dbContext.SaveChanges();

        return entry;
    }

    public IReadOnlyCollection<ShortUrlEntry> GetAll() => _dbContext.ShortUrls
        .AsNoTracking()
        .OrderByDescending(x => x.CreatedAtUtc)
        .ToList();

    private string GenerateUniqueCode()
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789";

        for (var attempt = 0; attempt < 10; attempt++)
        {
            var code = new string(Enumerable.Range(0, 7).Select(_ => chars[Random.Shared.Next(chars.Length)]).ToArray());
            var exists = _dbContext.ShortUrls.AsNoTracking().Any(x => x.Code == code);
            if (!exists)
            {
                return code;
            }
        }

        throw new InvalidOperationException("Unable to generate a unique short URL code.");
    }
}
