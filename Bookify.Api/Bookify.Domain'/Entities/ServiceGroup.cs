using Bookify.Domain_.Common;

namespace Bookify.Domain_.Entities;

public class ServiceGroup : AuditableEntity
{
    public virtual ICollection<Service> Services { get; set; }
    public virtual ICollection<ServiceGroupTranslation> ServiceGroupTranslations { get; set; }
}
