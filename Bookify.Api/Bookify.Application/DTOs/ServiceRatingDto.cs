﻿using Bookify.Domain_.Entities;
using Bookify.Domain_.Enums;

namespace Bookify.Application.DTOs;

public record ServiceRatingDto(
    int Id,
    string? Comment,
    string? TicketNumber,
    string? DeskNumber,
    string? DeskName,
    SmileyRating? SmileyRating,
    int? PredefinedTextId,
    PredefinedText? PredefinedText,
    int? BookingId,
    BookingDto? Booking,
    int? ETicketId,
    ETicketDto? ETicket,
    int? ServiceId,
    ServiceDto? Service
    );
