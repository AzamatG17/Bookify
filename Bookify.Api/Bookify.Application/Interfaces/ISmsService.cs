using Bookify.Application.Models;

namespace Bookify.Application.Interfaces;

public interface ISmsService
{
    Task SendMessage(SmsMessage message);
}
