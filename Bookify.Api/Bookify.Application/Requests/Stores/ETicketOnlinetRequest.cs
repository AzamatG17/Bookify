namespace Bookify.Application.Requests.Stores;

public class ETicketOnlinetRequest
{
    public string branchId { get; set; }
    public string? deviceId { get; set; }
    public int? deviceType { get; set; }
    public string languageId { get; set; }
    public string phoneNumber { get; set; }
    public int serviceId { get; set; }
}
