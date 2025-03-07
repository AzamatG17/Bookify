using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bookify.Application.DTOs;
using Bookify.Application.Interfaces.IServices;
using Bookify.Application.Interfaces.Services;
using Bookify.Domain_.Entities;
using Bookify.Domain_.Exceptions;
using Bookify.Domain_.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Application.Services;

public sealed class ServiceRatingService : IServiceRatingService
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public ServiceRatingService(IApplicationDbContext context, IMapper mapper, UserManager<User> userManager, ICurrentUserService currentUserService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context)); 
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); 
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
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

        var user = await GetUserAsync(_currentUserService.GetUserId());

        var newServiceRating = new ServiceRating
        {
            Comment = createDto.Comment,
            TicketNumber = createDto.TicketNumber,
            DeskNumber = createDto.DeskNumber,
            DeskName = createDto.DeskName,
            SmileyRating = createDto.SmileyRating,
            PredefinedTextId = createDto.PredefinedTextId == 0 || createDto.PredefinedTextId == null ? (int?)null : createDto.PredefinedTextId,
            ServiceId = createDto.ServiceId == 0 || createDto.ServiceId == null ? (int?)null : createDto.ServiceId,
            UserId = user.Id,
        };

        if (createDto.BookingId.HasValue && createDto.BookingId > 0)
        {
            newServiceRating.BookingId = await _context.Bookings
                .Where(x => x.BookingId == createDto.BookingId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
        }

        if (createDto.ETicketId.HasValue && createDto.ETicketId > 0)
        {
            newServiceRating.ETicketId = await _context.Etickets
                .Where(x => x.ETicketId == createDto.ETicketId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
        }

        _context.ServiceRatings.Add(newServiceRating);
        await _context.SaveChangesAsync();

        return _mapper.Map<ServiceRatingDto>(newServiceRating);
    }

    private async Task<User> GetUserAsync(Guid userId)
    {
        return await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == userId)
            ?? throw new EntityNotFoundException($"User with id: {userId} does not exist.");
    }
}
