using Bookify.Domain_.Common;

namespace Bookify.Domain_.Entities;

public class Service : AuditableEntity
{
    public required int ServiceId { get; set; }

    public int? BranchId { get; set; }
    public virtual Branch Branch { get; set; }
    public int? ServiceGroupId { get; set; }
    public virtual ServiceGroup ServiceGroup { get; set; }
    public virtual ICollection<Booking> Bookings { get; set; }
    public virtual ICollection<ETicket> ETickets { get; set; }
    public virtual ICollection<ServiceTranslation> ServiceTranslations { get; set; }
}
