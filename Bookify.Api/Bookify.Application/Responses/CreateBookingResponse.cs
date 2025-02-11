namespace Bookify.Application.Responses;

public class CreateBookingResponse
{
    public int BookingId { get; set; }
    public string BookingCode { get; set; }
    public DateTime BookingDate { get; set; }
    public TimeSpan BookingTime { get; set; }
    public string BranchName { get; set; }
    public string ServiceName { get; set; }
    public bool Success { get; set; }
    public string? ClientId { get; set; }
}
