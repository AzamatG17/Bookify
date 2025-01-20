using System.ComponentModel.DataAnnotations;

namespace Bookify.Application.Configurations;

public sealed class BotTokenOptions
{
    public const string SectionName = nameof(BotTokenOptions);

    [Required]
    public required string TelegramToken { get; init; }
}
