using Bookify.Domain_.Common;

namespace Bookify.Domain_.Entities;

public class Services : AuditableEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Location { get; set; }
    public required int CompanyId { get; set; }
    public virtual required Companies Company { get; set; }
}
