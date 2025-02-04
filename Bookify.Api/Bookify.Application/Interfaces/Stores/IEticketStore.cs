using Bookify.Application.Requests.Stores;
using Bookify.Application.Responses;

namespace Bookify.Application.Interfaces.Stores;

public interface IEticketStore
{
    Task<EticketResponse> CreateTicketForBookingServiceAsync(EticketRequest request, string baseUrl);
    Task<EticketResponse> CreateTicketForOnlinetAsync(ETicketOnlinetRequest request, string baseUrl);
}
