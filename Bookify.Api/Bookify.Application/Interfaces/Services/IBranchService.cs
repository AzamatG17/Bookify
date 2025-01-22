using Bookify.Application.DTOs;
using Bookify.Application.Requests.Services;

namespace Bookify.Application.Interfaces.IServices;

public interface IBranchService
{
    Task<List<CompanyWithBranchesDto>> GetAllAsync();
    Task<CompanyWithBranchesDto> GetByIdAsync(CompanyRequest companyRequest);
}
