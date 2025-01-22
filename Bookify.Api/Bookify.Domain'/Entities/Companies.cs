using Bookify.Domain_.Common;
using Bookify.Domain_.Enums;

namespace Bookify.Domain_.Entities;

public class Companies : AuditableEntity
{
    public required string Name { get; set; }
    public required string BaseUrl { get; set; }
    public required Projects Projects { get; set; }
    public string? Color { get; set; }
    public string? BackgroundColor { get; set; }

    public virtual ICollection<Branch> Branches { get; set; }
}
