namespace Bookify.Application.DTOs;

public record FreeDayDto(
    DateTime Date,
    int Day,
    int Month,
    int Year,
    int FreePlacesCount,
    int TotalPlacesCount
    );
