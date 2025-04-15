using Bookify.Application.DTOs;
namespace Bookify.Application.Requests.Services;

public record ServiceGroupRequest(
    List<ServiceGroupTranslationDto> ServiceGroupTranslationDtos
    );