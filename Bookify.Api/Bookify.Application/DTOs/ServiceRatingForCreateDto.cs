using Bookify.Domain_.Enums;

namespace Bookify.Application.DTOs;

public record ServiceRatingForCreateDto(
    string? Comment,
    string? TicketNumber,
    string? DeskNumber,
    string? DeskName,
    SmileyRating? SmileyRating,
    int? PredefinedTextId,
    int? BookingId,
    int? ETicketId,
    int? ServiceId
    );
