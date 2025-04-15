using Bookify.Application.DTOs;
using Bookify.Application.QueryParameters;
using Bookify.Application.Requests.Services;

namespace Bookify.Application.Interfaces.Services;

public interface IServiceGroupService
{
    Task<List<ServiceGroupDto>> GetAllAsync(ServiceGroupQueryParameters parameters);
    Task<ServiceGroupForCreateDto> CreateAsync(ServiceGroupRequest request);
    Task<ServiceGroupDto> UpdateAsync(ServiceGroupDto serviceGroup);
    Task DeleteAsync(int id);
}
