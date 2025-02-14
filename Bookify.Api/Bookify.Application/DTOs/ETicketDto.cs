namespace Bookify.Application.DTOs;

public record ETicketDto(
    int ETicketId,
    string Number,
    string Message,
    string ServiceName,
    int? BranchId,
    string BranchName,
    string BranchAddress,
    string ValidUntil
    );
