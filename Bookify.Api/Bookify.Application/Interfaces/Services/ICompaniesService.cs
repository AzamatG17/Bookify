using Bookify.Application.DTOs;
using Bookify.Application.Requests.Services;

namespace Bookify.Application.Interfaces.Services;

public interface ICompaniesService
{
    Task<List<CompaniesDto>> GetAllAsync();
    Task<CompaniesDto> GetByIdAsync(CompanyRequest request);
    Task<CompaniesDto> CreateAsync(CreateCompanyRequest request);
    Task<CompaniesDto> UpdateAsync(UpdateCompanyRequest request);
    Task DeleteAsync(CompanyRequest companyRequest);
}
