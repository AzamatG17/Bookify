namespace Bookify.Application.Responses;

public class BookingOnlinetResponse
{
    public string BookingCode { get; set; }
    public string BookingDate { get; set; }
    public string BookingDateTime { get; set; }
    public int BookingId { get; set; }
    public bool BookingIsForClerk { get; set; }
    public string BookingTime { get; set; }
    public int BranchId { get; set; }
    public string BranchName { get; set; }
    public int ClerkId { get; set; }
    public string ClerkName { get; set; }
    public string ClientId { get; set; }
    public string CustomerId { get; set; }
    public string Email { get; set; }
    public bool Expired { get; set; }
    public List<object> Files { get; set; }
    public string FirstName { get; set; }
    public int LanguageId { get; set; }
    public string LanguageShortId { get; set; }
    public string LastName { get; set; }
    public string Name { get; set; }
    public string Note { get; set; }
    public List<object> OrderNumbers { get; set; }
    public int Palette { get; set; }
    public int ParentServiceId { get; set; }
    public string ParentServiceName { get; set; }
    public string PhoneNumber { get; set; }
    public string PlateNumber1 { get; set; }
    public string PlateNumber2 { get; set; }
    public int ResultCode { get; set; }
    public int ServiceId { get; set; }
    public string ServiceName { get; set; }
    public int Status { get; set; }
    public bool Success { get; set; }
    public int VehicleType { get; set; }
}
