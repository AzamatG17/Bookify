using Bookify.Application.DTOs;

namespace Bookify.Application.Interfaces.Services;

public interface IServiceRatingService
{
    Task<List<ServiceRatingDto>> GetAllServiceRatingAsync();
    Task<ServiceRatingDto> CreateServiceRatingAsync(ServiceRatingForCreateDto serviceRatingForCreateDto);
}
