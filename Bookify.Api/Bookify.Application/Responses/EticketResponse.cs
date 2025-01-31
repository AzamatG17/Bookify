namespace Bookify.Application.Responses;

public class EticketResponse
{
    public string BranchAddress { get; set; }
    public int BranchId { get; set; }
    public string BranchName { get; set; } = null;
    public int Code { get; set; }
    public string CreatedTime { get; set; } = null;
    public string EstimatedWaitingTime { get; set; } = null;
    public string Message { get; set; } = null;
    public string Number { get; set; }
    public string Service { get; set; } = null;
    public bool ShowArriveButton { get; set; }
    public bool Success { get; set; }
    public int TicketId { get; set; }
    public string ValidUntil { get; set; } = null;
    public int WaitingCount { get; set; }
}
