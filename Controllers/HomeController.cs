using Microsoft.AspNetCore.Mvc;
using UI_Test.Models;
using UI_Test.Services;

namespace UI_Test.Controllers;

public class HomeController(IUrlShortenerService shortenerService) : Controller
{
    private readonly IUrlShortenerService _shortenerService = shortenerService;

    [HttpGet]
    public IActionResult Index() => View(_shortenerService.GetAll());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateShortUrlRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", _shortenerService.GetAll());
        }

        var entry = _shortenerService.Create(request.Url);
        TempData["CreatedShortUrl"] = $"{Request.Scheme}://{Request.Host}/s/{entry.Code}";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("s/{code}")]
    public IActionResult Resolve(string code)
    {
        var entry = _shortenerService.GetByCode(code);
        if (entry is null)
        {
            return NotFound("Short URL not found.");
        }

        return Redirect(entry.OriginalUrl);
    }
}
