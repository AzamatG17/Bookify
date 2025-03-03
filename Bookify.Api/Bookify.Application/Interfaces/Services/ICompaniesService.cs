using Bookify.Application.DTOs;
using Bookify.Application.Requests.Services;

namespace Bookify.Application.Interfaces.Services;

public interface ICompaniesService
{
    Task<List<CompaniesDto>> GetAllAsync();
    Task<List<CompaniesForAdminDto>> GetAllForAdminAsync();
    Task<CompaniesForAdminDto> GetByIdAsync(CompanyRequest request);
    Task<CompaniesForAdminDto> CreateAsync(CreateCompanyRequest request);
    Task<CompaniesForAdminDto> UpdateAsync(UpdateCompanyRequest request);
    Task DeleteAsync(CompanyRequest companyRequest);
}
