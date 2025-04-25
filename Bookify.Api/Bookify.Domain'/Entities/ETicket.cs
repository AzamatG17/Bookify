using Bookify.Domain_.Common;

namespace Bookify.Domain_.Entities;

public class ETicket : AuditableEntity
{
    public int ETicketId { get; set; }
    public string Language { get; set; }
    public string ServiceName { get; set; }
    public string BranchName { get; set; }
    public DateTime CreatedTime { get; set; }
    public string Message { get; set; }
    public string Number { get; set; }
    public bool Success { get; set; }
    public bool ShowArriveButton { get; set; }
    public int TicketId { get; set; }
    public string ValidUntil { get; set; }

    public int ServiceId { get; set; }
    public virtual Service Service { get; set; }
    public required Guid UserId { get; set; }
    public virtual User User { get; set; }
    public virtual ServiceRating? ServiceRating { get; set; }
}
