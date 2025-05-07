using AutoMapper;
using Bookify.Application.DTOs;
using Bookify.Application.Interfaces.Services;
using Bookify.Application.QueryParameters;
using Bookify.Application.Requests.Services;
using Bookify.Domain_.Entities;
using Bookify.Domain_.Exceptions;
using Bookify.Domain_.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Application.Services;

internal sealed class ServiceGroupService(IApplicationDbContext context, IMapper mapper) : IServiceGroupService
{
    private readonly IApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<List<ServiceGroupDto>> GetAllAsync(ServiceGroupQueryParameters parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        var query = await FilterServiceGroup(parameters);

        return query.Select(MapToServiceGroupDto).ToList();
    }

    public async Task<ServiceGroupForCreateDto> CreateAsync(ServiceGroupRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var serviceGroup = new ServiceGroup
        {
            ServiceGroupTranslations = request.ServiceGroupTranslationDtos.Select(t => new ServiceGroupTranslation
            {
                Name = t.Name,
                LanguageCode = t.LanguageCode,

            }).ToList()
        };

        _context.ServiceGroups.Add(serviceGroup);
        await _context.SaveChangesAsync();

        return MapToServiceGroupDto(serviceGroup);
    }

    public async Task<ServiceGroupDto> UpdateAsync(ServicegroupUpdateDto serviceGroup)
    {
        ArgumentNullException.ThrowIfNull(serviceGroup);

        var existingServiceGroup = await _context.ServiceGroups
            .Include(sg => sg.Services)
            .Include(sg => sg.ServiceGroupTranslations)
            .FirstOrDefaultAsync(sg => sg.Id == serviceGroup.Id)
            ?? throw new EntityNotFoundException($"ServiceGroup с идентификатором {serviceGroup.Id} не найдена.");

        var newServices = await _context.Services
            .Where(s => serviceGroup.ServiceIds.Contains(s.Id))
            .ToListAsync();

        existingServiceGroup.Services.Clear();
        foreach (var service in newServices)
        {
            existingServiceGroup.Services.Add(service);
        }

        await _context.SaveChangesAsync();

        return _mapper.Map<ServiceGroupDto>(existingServiceGroup);
    }

    public async Task DeleteAsync(int id)
    {
        var serviceGroup = await _context.ServiceGroups
            .FirstOrDefaultAsync(s => s.Id == id)
            ?? throw new EntityNotFoundException($"ServiceGroup с идентификатором {id} не найдена.");

        _context.ServiceGroups.Remove(serviceGroup);
        await _context.SaveChangesAsync();
    }

    private async Task<List<ServiceGroupTranslation>> FilterServiceGroup(ServiceGroupQueryParameters parameters)
    {
        var query = _context.ServiceGroupTranslations
            .Include(s => s.ServiceGroup)
                .ThenInclude(s => s.Services)
                    .ThenInclude(b => b.Branch)
                        .ThenInclude(c => c.Companies)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(parameters.Language))
        {
            query = query.Where(q => q.LanguageCode != null && q.LanguageCode.Contains(parameters.Language));
        }

        if (!string.IsNullOrEmpty(parameters.Search))
        {
            query = query.Where(q => q.Name != null && q.Name.Contains(parameters.Search));
        }

        if (parameters.CompanyId.HasValue)
        {
            query = query.Where(q =>
                q.ServiceGroup.Services.Any(s => s.Branch != null && s.Branch.CompanyId == parameters.CompanyId));
        }

        query = parameters.SortBy switch
        {
            "idDesc" => query.OrderByDescending(q => q.Id),
            "idAsc" => query.OrderBy(q => q.Id),
            _ => query
        };

        return await query.ToListAsync();
    }

    private static ServiceGroupDto MapToServiceGroupDto(ServiceGroupTranslation translation)
    {
        return new ServiceGroupDto(
            translation.ServiceGroupId,
            translation.ServiceGroup.Services.Select(b => b.Branch.Companies.Color).FirstOrDefault(),
            translation.Name
        );
    }

    private static ServiceGroupForCreateDto MapToServiceGroupDto(ServiceGroup serviceGroup)
    {
        return new ServiceGroupForCreateDto(
            serviceGroup.Id,
            serviceGroup.Services.Select(b => b.Branch.Companies.Color).FirstOrDefault(),
            serviceGroup.ServiceGroupTranslations
                .Select(t => new ServiceGroupTranslationDto(
                    t.Name,
                    t.LanguageCode
                ))
                .ToList()
            );
    }
}
