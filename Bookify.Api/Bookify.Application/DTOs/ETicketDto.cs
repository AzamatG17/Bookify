namespace Bookify.Application.DTOs;

public record ETicketDto(
    int ETicketId,
    string Number,
    string Message,
    string ServiceName,
    int? BranchId,
    int? SecondBranchId,
    string BranchName,
    string BranchAddress,
    string ValidUntil
    );
