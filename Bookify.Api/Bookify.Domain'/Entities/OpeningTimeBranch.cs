using Bookify.Domain_.Common;

namespace Bookify.Domain_.Entities;

public class OpeningTimeBranch : AuditableEntity
{
    public required int Day { get; set; }
    public required string OpenTime { get; set; }
    public required TimeSpan StartTime { get; set; }
    public required TimeSpan EndTime { get; set; }
}
