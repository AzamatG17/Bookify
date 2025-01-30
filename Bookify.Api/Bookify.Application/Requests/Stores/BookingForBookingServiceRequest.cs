namespace Bookify.Application.Requests.Stores;

public class BookingForBookingServiceRequest
{
    public int BranchId { get; set; }
    public string? Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string LanguageShortId { get; set; }
    public string PhoneNumber { get; set; }
    public int ServiceId { get; set; }
    public DateTime StartDate { get; set; }
    public string StartTime { get; set; }
}
