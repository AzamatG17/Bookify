namespace Bookify.Application.Responses;

public class CreateBookingResponse
{
    public string BookingCode { get; set; }
    public string BookingDate { get; set; }
    public string BookingDateTime { get; set; }
    public int BookingId { get; set; }
    public bool Success { get; set; }
    public string ServiceName { get; set; }
    public string BranchName { get; set; }
}
