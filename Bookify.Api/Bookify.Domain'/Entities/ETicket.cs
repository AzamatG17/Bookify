using Bookify.Domain_.Common;

namespace Bookify.Domain_.Entities;

public class ETicket :AuditableEntity
{
    public int ServiceId { get; set; }
    public string Language { get; set; }
    public string ServiceName { get; set; }
    public string BranchName { get; set; }
    public string CreatedTime { get; set; }
    public string Message { get; set; }
    public string Number { get; set; }
    public bool Success { get; set; }
    public bool ShowArriveButton { get; set; }
    public int TicketId { get; set; }
    public string ValidUntil { get; set; }

    public required Guid UserId { get; set; }
    public virtual User User { get; set; }
}

//{
//    "BranchAddress": "ул. Дагбитская, 14",
//    "BranchId": 2009,
//    "BranchName": "Региональный филиал \"Умар\"",
//    "Code": 0,
//    "CreatedTime": "/Date(1738263600000+0500)/",
//    "EstimatedWaitingTime": null,
//    "Message": "Билет создан.",
//    "Number": "A243",
//    "Service": "КАРТЫ VISA/CUP/MC",
//    "ShowArriveButton": false,
//    "Success": true,
//    "TicketId": 97896,
//    "ValidUntil": "2/1/2025 12:00:00 AM",
//    "WaitingCount": 0
//}
