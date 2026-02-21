using System.Collections.Concurrent;
using UI_Test.Models;

namespace UI_Test.Services;

public class UrlShortenerService : IUrlShortenerService
{
    private readonly ConcurrentDictionary<string, ShortUrlEntry> _store = new();

    public ShortUrlEntry Create(string url)
    {
        var code = GenerateCode();
        var entry = new ShortUrlEntry
        {
            Code = code,
            OriginalUrl = url,
            CreatedAtUtc = DateTime.UtcNow,
            VisitCount = 0
        };

        _store[code] = entry;
        return entry;
    }

    public ShortUrlEntry? GetByCode(string code)
    {
        if (!_store.TryGetValue(code, out var entry))
        {
            return null;
        }

        entry.VisitCount++;
        return entry;
    }

    public IReadOnlyCollection<ShortUrlEntry> GetAll() => _store.Values.OrderByDescending(x => x.CreatedAtUtc).ToList();

    private string GenerateCode()
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789";
        return new string(Enumerable.Range(0, 7).Select(_ => chars[Random.Shared.Next(chars.Length)]).ToArray());
    }
}
