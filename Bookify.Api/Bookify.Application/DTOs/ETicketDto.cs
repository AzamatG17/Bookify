using Bookify.Domain_.Enums;

namespace Bookify.Application.DTOs;

public record ETicketDto(
    int ETicketId,
    string Number,
    string Message,
    string ServiceName,
    int? BranchId,
    int? SecondBranchId,
    Projects? Projects,
    string BranchName,
    string BranchAddress,
    string ValidUntil
    );
