namespace Bookify.Application.Requests.Services;

public record GetBookingStatusRequest(string BookingCode, int SecondBranchId);
