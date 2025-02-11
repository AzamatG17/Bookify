namespace Bookify.Application.Requests.Stores;

public class DeleteBookingRequest
{
    public string BookingCode { get; set; }
    public string ClientId { get; set; }
    public string LanguageShortId { get; set; }
    public string StartDate { get; set; }
}
