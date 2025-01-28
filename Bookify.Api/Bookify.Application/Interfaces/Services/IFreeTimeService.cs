using Bookify.Application.DTOs;
using Bookify.Application.Requests.Services;

namespace Bookify.Application.Interfaces.Services;

public interface IFreeTimeService
{
    Task<List<FreeDayDto>> GetFreeDayListAsync(FreeTimeRequest freeTimeRequest);
    Task<List<FreeTimeDto>> GetFreeTimeListAsync(FreeTimeRequest freeTimeRequest);
}
