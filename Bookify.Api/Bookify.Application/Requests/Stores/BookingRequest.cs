namespace Bookify.Application.Requests.Stores;

public class BookingRequest
{
    public string branchId { get; set; }
    public string customerID { get; set; } = "";
    public string email { get; set; }
    public string firstName { get; set; }
    public string languageShortId { get; set; }
    public string lastName { get; set; }
    public string name { get; set; }
    public string note { get; set; } = "";
    public string phoneNumber { get; set; }
    public string serviceId { get; set; }
    public string startDate { get; set; } // формат "yyyyMMdd"
    public string startTime { get; set; } // формат "HH:mm:ss"
}
