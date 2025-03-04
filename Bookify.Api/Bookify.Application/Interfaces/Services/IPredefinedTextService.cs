using Bookify.Application.DTOs;
using Bookify.Application.Requests.Services;

namespace Bookify.Application.Interfaces.Services;

public interface IPredefinedTextService
{
    Task<List<PredefinedTextDto>> GetAllAsync();
    Task<PredefinedTextDto> CreateAsync(PredefinedTextForCreateDto textDto);
    Task<PredefinedTextDto> UpdateAsync(PredefinedTextDto textDto);
    Task DeleteAsync(PredefinedTextRequest predefinedTextRequest);
}
