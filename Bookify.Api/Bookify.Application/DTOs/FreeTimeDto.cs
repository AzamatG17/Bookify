namespace Bookify.Application.DTOs;

public record FreeTimeDto(
    string Time,
    bool IsAvailable,
    int FreePlaces,
    int TotalPlaces
    );
