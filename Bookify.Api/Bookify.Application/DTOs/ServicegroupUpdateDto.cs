namespace Bookify.Application.DTOs;

public record ServicegroupUpdateDto(
    int Id,
    List<int> ServiceIds
    );
