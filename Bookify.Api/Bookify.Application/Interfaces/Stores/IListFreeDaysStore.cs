using Bookify.Application.DTOs;

namespace Bookify.Application.Interfaces.Stores;

public interface IListFreeDaysStore
{
    Task<List<FreeDayDto>> GetDataBookingServiceAsync(int branchId, int serviceId, DateTime startDate, string baseUrl);
    Task<List<FreeDayDto>> GetDataOnlinetAsync(int branchId, int serviceId, DateTime startDate, string baseUrl);
}
