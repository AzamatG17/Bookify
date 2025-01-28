using Bookify.Application.DTOs;

namespace Bookify.Application.Interfaces.Stores;

public interface IListFreeTimesStore
{
    Task<List<FreeTimeDto>> GetDataBookingServiceAsync(int branchId, int serviceId, DateTime startDate, string baseUrl);
    Task GetDataOnlinetAsync(int branchId, int serviceId, DateTime startDate);
}
