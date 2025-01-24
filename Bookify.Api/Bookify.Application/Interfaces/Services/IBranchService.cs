using Bookify.Application.DTOs;
using Bookify.Application.Requests.Services;
using Bookify.Domain_.Entities;

namespace Bookify.Application.Interfaces.IServices;

public interface IBranchService
{
    Task<List<CompanyWithBranchesDto>> GetAllAsync();
    Task<CompanyWithBranchesDto> GetByIdAsync(CompanyRequest companyRequest);
    Task<List<Branch>> UpdateDateAsync(CompanyRequest request);
}
