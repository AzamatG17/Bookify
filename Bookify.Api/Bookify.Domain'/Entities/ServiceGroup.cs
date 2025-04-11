using Bookify.Domain_.Common;

namespace Bookify.Domain_.Entities;

public class ServiceGroup : AuditableEntity
{
    public required string Name { get; set; }
    public virtual ICollection<Service> Services { get; set; }
}
