using Bookify.Domain_.Entities;

namespace Bookify.Application.Interfaces.Stores;

public interface IServiceStore
{
    Task<List<Service>> GetDataOnlinetAsync(Branch branch, List<string> languages);
    Task<List<Service>> GetDataBookingServiceAsync(Branch branch, List<string> languages);
}
