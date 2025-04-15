namespace Bookify.Application.DTOs;

public record ServiceGroupForCreateDto(
    int Id,
    string Color,
    List<ServiceGroupTranslationDto> ServiceGroups
    );
