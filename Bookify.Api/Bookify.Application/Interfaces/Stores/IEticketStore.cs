using Bookify.Application.Requests.Services;
using Bookify.Application.Requests.Stores;
using Bookify.Application.Responses;

namespace Bookify.Application.Interfaces.Stores;

public interface IEticketStore
{
    Task<object> GetEtickertStatusBookingServiceAsync(EticketStatusRequest request, string baseUrl);
    Task<EticketResponse> CreateTicketForBookingServiceAsync(EticketRequest request, string baseUrl);
    Task<EticketResponse> CreateTicketForOnlinetAsync(ETicketOnlinetRequest request, string baseUrl);
    Task<EticketDeleteStatus> DeleteBookingServiceAsync(string baseUrl, int branchId, string number);
}
