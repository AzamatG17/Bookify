using Bookify.Application.Requests.Services;
using Bookify.Domain_.Exceptions;
using Bookify.Domain_.Interfaces;
using Bookify.Domain_.Entities;
using Microsoft.EntityFrameworkCore;
using Bookify.Application.Interfaces.Stores;
using Bookify.Application.Interfaces.Services;
using Bookify.Application.DTOs;
using Bookify.Application.QueryParameters;

namespace Bookify.Application.Services;

internal sealed class ServicesService : IServicesService
{
    private readonly IApplicationDbContext _context;
    private readonly IServiceStore _serviceStore;

    public ServicesService(IApplicationDbContext context, IServiceStore serviceStore)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _serviceStore = serviceStore ?? throw new ArgumentNullException(nameof(serviceStore));
    }

    public async Task<List<ServiceDto>> GetAllAsync(ServiceQueryParameters serviceQueryParameters)
    {
        ArgumentNullException.ThrowIfNull(nameof(serviceQueryParameters));

        var query = FilterService(serviceQueryParameters);

        var services = await query;

        return services.Select(service => MapToServiceDto(service)).ToList();
    }

    public async Task<ServiceDto> GetByIdAsync(ServiceByIdQueryParameters serviceByIdQueryParameters)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ServiceDto>> UpdateDataAsync(BranchRequest branchRequest)
    {
        ArgumentNullException.ThrowIfNull(nameof(branchRequest));

        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            var services = await _context.Services
                .Where(s => s.BranchId == branchRequest.BranchId)
                .ToListAsync();

            if (services.Any())
            {
                _context.Services.RemoveRange(services);
            }

            var newServices = await GetDataForServiceStore(branchRequest);

            await _context.Services.AddRangeAsync(newServices);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return newServices.Select(MapToServiceDto).ToList();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            throw new InvalidUpdateDataException("Failed to update branch services. See inner exception for details.", ex);
        }
    }

    private async Task<List<Service>> GetDataForServiceStore(BranchRequest branchRequest)
    {
        var branch = await _context.Branches
            .Select(x => new Branch
            {
                Id = x.Id,
                BranchId = x.BranchId,
                Name = x.Name,
                Companies = new Companies
                {
                    Projects = x.Companies.Projects,
                    Name = x.Companies.Name,
                    BaseUrl = x.Companies.BaseUrl
                }
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == branchRequest.BranchId);

        if (branch is null)
        {
            throw new EntityNotFoundException($"Branch with id:{branchRequest.BranchId} does not exist.");
        }

        List<Service> services = [];
        List<string> Language = new()
        {
            "en", "ru", "uz"
        };

        if (branch.Companies.Projects == Domain_.Enums.Projects.BookingService)
        {
            services = await _serviceStore.GetDataBookingServiceAsync(branch, Language);
        }
        else if (branch.Companies.Projects == Domain_.Enums.Projects.Onlinet)
        {
            services = await _serviceStore.GetDataOnlinetAsync(branch, Language);
        }

        return services.ToList();
    }

    private async Task<List<ServiceTranslation>> FilterService(ServiceQueryParameters serviceQuery)
    {
        var query = _context.ServiceTranslations
            .Include(s => s.Services)
            .ThenInclude(b => b.Branch)
            .ThenInclude(c => c.Companies)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(serviceQuery.Language))
        {
            query = query.Where(q => q.LanguageCode != null && q.LanguageCode.Contains(serviceQuery.Language));
        }

        if (!string.IsNullOrEmpty(serviceQuery.Search))
        {
            query = query.Where(q => q.Name != null && q.Name.Contains(serviceQuery.Search));
        }

        if (serviceQuery.CompanyId.HasValue)
        {
            query = query.Where(q => q.Services.Branch.CompanyId == serviceQuery.CompanyId);
        }

        query = serviceQuery.SortBy switch
        {
            "idDesc" => query.OrderByDescending(q => q.Id),
            "idAsc" => query.OrderBy(q => q.Id),
            _ => query
        };

        return await query.ToListAsync();
    }

    private static ServiceDto MapToServiceDto(ServiceTranslation service)
    {
        return new ServiceDto(
            service.Id,
            service.Services.ServiceId,
            service.Name,
            service.Services.Branch.Companies.Id
        );
    }

    private static ServiceDto MapToServiceDto(Service service)
    {
        return new ServiceDto(
            service.Id,
            service.ServiceId,
            "",
            0
            );
    }
}
