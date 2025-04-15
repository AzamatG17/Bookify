using Bookify.Application.DTOs;
using Bookify.Application.Interfaces.Services;
using Bookify.Application.Interfaces.Stores;
using Bookify.Application.QueryParameters;
using Bookify.Application.Requests.Services;
using Bookify.Domain_.Entities;
using Bookify.Domain_.Exceptions;
using Bookify.Domain_.Interfaces;
using Microsoft.EntityFrameworkCore;

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
        ArgumentNullException.ThrowIfNull(serviceQueryParameters);

        var query = await FilterService(serviceQueryParameters);

        return query.Select(MapToServiceDto).ToList();
    }

    public async Task<ServiceDto> GetByIdAsync(ServiceByIdQueryParameters serviceByIdQueryParameters)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ServiceDto>> UpdateDataAsync(BranchRequest branchRequest)
    {
        ArgumentNullException.ThrowIfNull(branchRequest);

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var services = await _context.Services
                .Include(st => st.ServiceTranslations)
                .Where(s => s.BranchId == branchRequest.BranchId)
                .ToListAsync();

            var newServices = await GetDataForServiceStore(branchRequest);

            foreach (var serviceDto in newServices)
            {
                var result = services.FirstOrDefault(x => x.ServiceId == serviceDto.ServiceId);
                if (result is not null)
                {
                    _context.ServiceTranslations.RemoveRange(result.ServiceTranslations);
                    result.ServiceTranslations = serviceDto.ServiceTranslations;
                }
                else
                {
                    await _context.Services.AddAsync(serviceDto);
                }
            }

            var serviceToRemove = services
                .Where(s => !newServices.Any(ns => ns.ServiceId == s.ServiceId))
                .ToList();

            _context.Services.RemoveRange(serviceToRemove);

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

    public async Task UpdateServiceGroupAsync(UpdateServiceGroupRequest requests)
    {
        ArgumentNullException.ThrowIfNull(requests);

        var service = await _context.Services
                .Include(x => x.ServiceGroups)
                .FirstOrDefaultAsync(sg => sg.Id == requests.Id)
                ?? throw new EntityNotFoundException($"Услуга с идентификатором: {requests.Id} не существует.");

        var existingGroups = await _context.ServiceGroups
                .Where(g => requests.ServiceGroupIds.Contains(g.Id))
                .ToListAsync();

        var missingIds = requests.ServiceGroupIds.Except(existingGroups.Select(g => g.Id)).ToList();

        service.ServiceGroups.Clear();

        foreach (var group in existingGroups)
        {
            service.ServiceGroups.Add(group);
        }

        await _context.SaveChangesAsync();
    }

    private async Task<List<Service>> GetDataForServiceStore(BranchRequest branchRequest)
    {
        var branch = await _context.Branches
            .Select(x => new Branch
            {
                Id = x.Id,
                BranchId = x.BranchId,
                Name = x.Name,
                Projects = x.Projects,
                Companies = new Companies
                {
                    Name = x.Companies.Name,
                    BaseUrlForOnlinet = x.Companies.BaseUrlForOnlinet,
                    BaseUrlForBookingService = x.Companies.BaseUrlForBookingService
                }
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == branchRequest.BranchId)
            ?? throw new EntityNotFoundException($"Branch with id:{branchRequest.BranchId} does not exist.");

        List<Service> services = [];
        List<string> Language = new()
        {
            "en", "ru", "uz"
        };

        if (branch.Projects == Domain_.Enums.Projects.BookingService)
        {
            services = await _serviceStore.GetDataBookingServiceAsync(branch, Language);
        }
        else if (branch.Projects == Domain_.Enums.Projects.Onlinet)
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

        if (serviceQuery.BranchId.HasValue)
        {
            query = query.Where(q => q.Services.BranchId == serviceQuery.BranchId);
        }

        if (serviceQuery.ServiceGroupId.HasValue)
        {
            query = query.Where(q => q.Services.ServiceGroups.Any(x => x.Id == serviceQuery.ServiceGroupId));
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
            service.Services.Id,
            service.Services.ServiceId,
            service.Name ?? "",
            service.Services.Branch.Companies.Color ?? "",
            service.Services.Branch?.Companies?.Id ?? 0,
            service.Services.Branch?.Companies?.Name ?? "",
            service.Services.Branch?.Id ?? 0,
            service.Services.Branch?.Name ?? "",
            service.Services.Branch?.CoordinateLatitude ?? 0.0,
            service.Services.Branch?.CoordinateLongitude ?? 0.0
        );
    }

    private static ServiceDto MapToServiceDto(Service service)
    {
        return new ServiceDto(
            service.Id,
            service.ServiceId,
            "",
            "",
            0,
            "",
            0,
            "",
            0,
            0
            );
    }
}
