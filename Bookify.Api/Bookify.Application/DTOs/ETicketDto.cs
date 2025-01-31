namespace Bookify.Application.DTOs;

public record ETicketDto(
    string Number,
    string Message,
    string ServiceName,
    string BranchName,
    string BranchAddress,
    string ValidUntil
    );
