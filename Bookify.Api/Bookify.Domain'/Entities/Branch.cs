using Bookify.Domain_.Common;

namespace Bookify.Domain_.Entities;

public class Branch : AuditableEntity
{
    public required int BranchId { get; set; }
    public required string Name { get; set; }
    public string? BranchAddres { get; set; }
    public double? CoordinateLatitude { get; set; }
    public double? CoordinateLongitude { get; set; }                                                                            

    public int CompanyId { get; set; }
    public virtual Companies Companies { get; set; }
    public virtual ICollection<OpeningTimeBranch> OpeningTimeBranches { get; set; }
    public virtual ICollection<Service> Services { get; set; }
}
