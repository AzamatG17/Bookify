using Bookify.Domain_.Entities;
using Bookify.Domain_.Enums;

namespace Bookify.Application.DTOs;

public record ServiceRatingByCompanyDto(
    int BranchId,
    string BranchName,
    List<ServiceRatingByBranchDto> ServiceRatingByBranchDtos
    );

public record ServiceRatingByBranchDto(
    int Id,
    string? Comment,
    SmileyRating? SmileyRating,
    int? PredefinedTextId,
    PredefinedText? PredefinedText,
    string UserName
    );
