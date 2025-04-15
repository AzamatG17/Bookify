using Bookify.Domain_.Common;

namespace Bookify.Domain_.Entities;

public class ServiceGroupTranslation : AuditableEntity
{
    public required string Name { get; set; }
    public required string LanguageCode { get; set; }

    public int ServiceGroupId { get; set; }
    public virtual ServiceGroup ServiceGroup { get; set; }
}
