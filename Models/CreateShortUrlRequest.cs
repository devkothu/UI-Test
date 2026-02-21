using System.ComponentModel.DataAnnotations;

namespace UI_Test.Models;

public class CreateShortUrlRequest
{
    [Required]
    [Url]
    public string Url { get; set; } = string.Empty;
}
