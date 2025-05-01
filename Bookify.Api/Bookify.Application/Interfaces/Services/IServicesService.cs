using Bookify.Application.DTOs;
using Bookify.Application.QueryParameters;
using Bookify.Application.Requests.Services;

namespace Bookify.Application.Interfaces.Services;

public interface IServicesService
{
    Task<List<ServiceWithRatingDto>> GetAllAsync(ServiceQueryParameters serviceQueryParameters);
    Task<ServiceDto> GetByIdAsync(ServiceByIdQueryParameters serviceQueryParameters);
    Task<List<ServiceDto>> UpdateDataAsync(BranchRequest branchRequest);
    Task UpdateServiceGroupAsync(UpdateServiceGroupRequest requests);
}
