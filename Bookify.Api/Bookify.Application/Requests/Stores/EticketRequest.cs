namespace Bookify.Application.Requests.Stores;

public class EticketRequest
{
    public int BranchId { get; set; }
    public int? DeviceType { get; set; }
    public string LanguageId { get; set; }
    public string PhoneNumber { get; set; }
    public int ServiceId { get; set; }
}
