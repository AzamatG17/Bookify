using Bookify.Domain_.Common;

namespace Bookify.Domain_.Entities;

public class Booking : AuditableEntity
{
    public int ServiceId { get; set; }
    public DateTime StartDate { get; set; }
    public string StartTime { get; set; }
    public string Language { get; set; }
    public string BookingCode { get; set; }
    public bool Success { get; set; }
    public string ServiceName { get; set; }
    public string BranchName { get; set; }

    public required Guid UserId { get; set; }
    public virtual User User { get; set; }  
}
