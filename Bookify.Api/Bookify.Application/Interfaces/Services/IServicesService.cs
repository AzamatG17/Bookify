using Bookify.Application.DTOs;
using Bookify.Application.QueryParameters;
using Bookify.Application.Requests.Services;
using Bookify.Domain_.Entities;

namespace Bookify.Application.Interfaces.Services;

public interface IServicesService
{
    Task<List<ServiceDto>> GetAllAsync(ServiceQueryParameters serviceQueryParameters);
    Task<ServiceDto> GetByIdAsync(ServiceByIdQueryParameters serviceQueryParameters);
    Task<List<Service>> UpdateDataAsync(BranchRequest branchRequest);
}
