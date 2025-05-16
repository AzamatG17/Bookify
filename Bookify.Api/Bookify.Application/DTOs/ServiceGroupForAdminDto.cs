namespace Bookify.Application.DTOs;

public record ServiceGroupForAdminDto(
    int Id,
    string Color,
    string Name,
    int CompanyId,
    string CompanyName,
    List<ServiceWithRatingDto> ServiceWithRatingDtos
    );
