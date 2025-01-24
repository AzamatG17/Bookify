using Bookify.Domain_.Common;

namespace Bookify.Domain_.Entities;

public class ServiceTranslation : AuditableEntity
{
    public required string Name { get; set; }
    public required string LanguageCode { get; set; }

    public required int ServiceId { get; set; }
    public virtual Service Services { get; set; }
}
