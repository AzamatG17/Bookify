using Bookify.Domain_.Common;
using Bookify.Domain_.Enums;

namespace Bookify.Domain_.Entities;

public class ServiceRating : AuditableEntity
{
    public string? Comment { get; set; }
    public string? TicketNumber { get; set; }
    public string? DeskNumber { get; set; }
    public string? DeskName { get; set; }
    public SmileyRating? SmileyRating { get; set; }

    public int? PredefinedTextId { get; set; }
    public virtual PredefinedText? PredefinedText { get; set; }
    public int? BookingId { get; set; }
    public virtual Booking? Booking { get; set; }
    public int? ETicketId { get; set; }
    public virtual ETicket? ETicket { get; set; }
    public int? ServiceId { get; set; }
    public virtual Service? Service { get; set; }
    public Guid? UserId { get; set; }
    public virtual User? User { get; set; }
}
