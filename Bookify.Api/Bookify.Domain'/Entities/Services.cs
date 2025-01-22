using Bookify.Domain_.Common;

namespace Bookify.Domain_.Entities;

public class Services : AuditableEntity
{
    public required string Name { get; set; }

    public required int BranchId { get; set; }
    public required virtual Branch Branch { get; set; }
}
