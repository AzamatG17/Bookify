﻿namespace Bookify.Application.DTOs;

public record BookingDto(
    int BookingId,
    string BookingCode,
    string ServiceName,
    int? BranchId,
    int? SecondBranchId,
    string BranchName,
    DateTime StartDate,
    string StartTime
    );
