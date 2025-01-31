using Bookify.Application.DTOs;
using Bookify.Application.Requests.Services;
using Bookify.Application.Responses;

namespace Bookify.Application.Interfaces.Services;

public interface IEticketService
{
    Task<ETicketDto> CreateTicketAsync(CreateEticketRequest request);
}
