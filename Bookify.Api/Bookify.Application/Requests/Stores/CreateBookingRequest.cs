namespace Bookify.Application.Requests.Stores;

public class CreateBookingRequest
{
    public string BranchId { get; set; }
    public string CustomerID { get; set; } = "";
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LanguageShortId { get; set; }
    public string LastName { get; set; }
    public string Name { get; set; }
    public string Note { get; set; } = "";
    public string PhoneNumber { get; set; }
    public string ServiceId { get; set; }
    public string StartDate { get; set; } // формат "yyyyMMdd"
    public string StartTime { get; set; } // формат "HH:mm:ss"
}
