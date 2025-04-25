using Bookify.Domain_.Entities;
using Bookify.Domain_.Enums;

namespace Bookify.Application.DTOs;

public record ServiceRatingWithOutEntityDto(
    int Id,
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
