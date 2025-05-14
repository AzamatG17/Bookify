using Bookify.Application.DTOs;
using Bookify.Application.QueryParameters;

namespace Bookify.Application.Interfaces.Services;

public interface IServiceRatingService
{
    Task<List<ServiceRatingDto>> GetAllServiceRatingAsync();
    Task<List<ServiceRatingByCompanyDto>> GetAllServiceRatingByCompanyAsync(ServiceRatingByCompanyQueryParametrs queryParametrs);
    Task<List<ServiceRatingDto>> GetServiceRatingAsync();
    Task<ServiceRatingDto> CreateServiceRatingAsync(ServiceRatingForCreateDto serviceRatingForCreateDto);
    Task UpdateAsync(ServiceRatingForUpdateDto dto);
    Task DeleteAsync(int id);
}
