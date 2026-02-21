using UI_Test.Models;

namespace UI_Test.Services;

public interface IUrlShortenerService
{
    ShortUrlEntry Create(string url);
    ShortUrlEntry? GetByCode(string code);
    IReadOnlyCollection<ShortUrlEntry> GetAll();
}
