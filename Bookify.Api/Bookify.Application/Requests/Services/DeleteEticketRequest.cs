namespace Bookify.Application.Requests.Services;

public record DeleteEticketRequest(int eTicketId, int SecondBranchId, string Language);
