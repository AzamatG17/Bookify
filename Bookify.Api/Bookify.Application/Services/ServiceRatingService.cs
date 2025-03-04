using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bookify.Application.DTOs;
using Bookify.Application.Interfaces.Services;
using Bookify.Domain_.Entities;
using Bookify.Domain_.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Application.Services;

public sealed class ServiceRatingService : IServiceRatingService
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ServiceRatingService(IApplicationDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context)); 
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));   
    }

    public async Task<List<ServiceRatingDto>> GetAllServiceRatingAsync()
    {
        var result = await _context.ServiceRatings
            .AsNoTracking()
            .ProjectTo<ServiceRatingDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return result ?? [];
    }

    public async Task<ServiceRatingDto> CreateServiceRatingAsync(ServiceRatingForCreateDto createDto)
    {
        ArgumentNullException.ThrowIfNull(createDto);

        var serviceRaing = _mapper.Map<ServiceRating>(createDto);

        _context.ServiceRatings.Add(serviceRaing);
        await _context.SaveChangesAsync();

        return _mapper.Map<ServiceRatingDto>(serviceRaing);
    }
}
