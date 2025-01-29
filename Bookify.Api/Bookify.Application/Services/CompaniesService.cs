using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bookify.Application.DTOs;
using Bookify.Application.Interfaces.Services;
using Bookify.Application.Requests.Services;
using Bookify.Domain_.Entities;
using Bookify.Domain_.Exceptions;
using Bookify.Domain_.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Application.Services;

internal sealed class CompaniesService(IApplicationDbContext context, IMapper mapper) : ICompaniesService
{
    private readonly IApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<List<CompaniesDto>> GetAllAsync()
    {
        var comapies = await _context.Companies
            .AsNoTracking()
            .ProjectTo<CompaniesDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return comapies ?? [];
    }

    public async Task<CompaniesDto> GetByIdAsync(CompanyRequest request)
    {
        var company = await _context.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id)
            ?? throw new EntityNotFoundException($"Company entity with id {request.Id} not found");

        return _mapper.Map<CompaniesDto>(company);
    }

    public async Task<CompaniesDto> CreateAsync(CreateCompanyRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var company = _mapper.Map<Companies>(request);

        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        return _mapper.Map<CompaniesDto>(company);
    }

    public async Task<CompaniesDto> UpdateAsync(UpdateCompanyRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var company = await _context.Companies
            .FirstOrDefaultAsync(c => c.Id == request.Id)
            ?? throw new EntityNotFoundException($"Company with id:{request.Id} does not exist.");

        _mapper.Map(request, company);

        _context.Companies.Update(company);
        await _context.SaveChangesAsync();

        return _mapper.Map<CompaniesDto>(company);
    }

    public async Task DeleteAsync(CompanyRequest companyRequest)
    {
        var company = await _context.Companies
            .FirstOrDefaultAsync(x => x.Id == companyRequest.Id)
            ?? throw new EntityNotFoundException($"Company with id: {companyRequest.Id} is not found.");

        _context.Companies.Remove(company);
        await _context.SaveChangesAsync();
    }
}
