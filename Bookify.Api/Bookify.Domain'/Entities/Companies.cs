using Bookify.Domain_.Common;

namespace Bookify.Domain_.Entities;

public class Companies : AuditableEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? BaseUrlForOnlinet { get; set; }
    public string? BaseUrlForBookingService { get; set; }
    public string? LogoBase64 { get; set; }
    public string? Color { get; set; }
    public string? BackgroundColor { get; set; }

    public virtual ICollection<Branch> Branches { get; set; }
}
