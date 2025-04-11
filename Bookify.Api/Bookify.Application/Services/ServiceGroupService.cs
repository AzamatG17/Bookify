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

internal sealed class ServiceGroupService(IApplicationDbContext context, IMapper mapper) : IServiceGroupService
{
    private readonly IApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<List<ServiceGroupDto>> GetAllAsync()
    {
        var serviceGroups = await _context.ServiceGroups
            .Include(s => s.Services)
            .AsNoTracking()
            .ProjectTo<ServiceGroupDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return serviceGroups;
    }

    public async Task<ServiceGroupDto> CreateAsync(ServiceGroupRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var serviceGroup = _mapper.Map<ServiceGroup>(request);

        await _context.ServiceGroups.AddAsync(serviceGroup);
        await _context.SaveChangesAsync();

        return _mapper.Map<ServiceGroupDto>(serviceGroup);
    }

    public async Task<ServiceGroupDto> UpdateAsync(ServiceGroupDto serviceGroup)
    {
        ArgumentNullException.ThrowIfNull(serviceGroup);

        var existingServiceGroup = await _context.ServiceGroups
            .FirstOrDefaultAsync(s => s.Id == serviceGroup.Id)
            ?? throw new EntityNotFoundException($"ServiceGroup с идентификатором {serviceGroup.Id} не найдена.");

        _mapper.Map(serviceGroup, existingServiceGroup);

        _context.ServiceGroups.Update(existingServiceGroup);
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
}
