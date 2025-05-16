using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bookify.Application.DTOs;
using Bookify.Application.Interfaces.IServices;
using Bookify.Application.Interfaces.Services;
using Bookify.Application.Models;
using Bookify.Application.QueryParameters;
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
    
    public async Task<List<ServiceRatingByCompanyDto>> GetAllServiceRatingByCompanyAsync(ServiceRatingByCompanyQueryParametrs queryParametrs)
    {
        ArgumentNullException.ThrowIfNull(queryParametrs);

        var query = await FilterServiceRating(queryParametrs);

        if (query == null || !query.Any())
        {
            throw new EntityNotFoundException("Рейтинги услуг по заданным параметрам не найдены.");
        }

        return MapServiceRatingsToDto(query);
    }

    public async Task<List<ServiceRatingDto>> GetServiceRatingAsync()
    {
        var user = await GetUserAsync(_currentUserService.GetUserId());

        var result = await _context.ServiceRatings
            .Where(x => x.UserId == user.Id)
            .Include(b => b.Booking)
            .Include(e => e.ETicket)
            .Include(s => s.Service)
            .AsNoTracking()
            .ToListAsync();

        return result.Select(MapToDto).ToList();
    }

    public async Task<ServiceRatingDto> CreateServiceRatingAsync(ServiceRatingForCreateDto createDto)
    {
        ArgumentNullException.ThrowIfNull(createDto);

        var user = await GetUserAsync(_currentUserService.GetUserId());

        if (await _context.ServiceRatings.AnyAsync(x =>
            x.Booking.BookingId == createDto.BookingId || x.ETicket.ETicketId == createDto.ETicketId))
        {
            throw new DuplicateBookingException("Вы уже оставили отзыв");
        }

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

    public async Task UpdateAsync(ServiceRatingForUpdateDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var user = await GetUserAsync(_currentUserService.GetUserId());

        var serviceRating = await _context.ServiceRatings
            .FirstOrDefaultAsync(s => s.Id == dto.Id)
            ?? throw new EntityNotFoundException($"ServiceRating с идентификатором: {dto.Id} не существует.");

        if (serviceRating.UserId != user.Id)
        {
            throw new Domain_.Exceptions.UnauthorizedAccessException("Вы не имеете доступа к этому отзыву.");
        }

        _mapper.Map(dto, serviceRating);

        if (dto.PredefinedTextId == 0)
            serviceRating.PredefinedTextId = null;

        if (dto.BookingId == 0)
        {
            serviceRating.BookingId = null;
        }
        else
        {
            serviceRating.BookingId = await _context.Bookings
                .Where(x => x.BookingId == dto.BookingId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
        }

        if (dto.ETicketId == 0)
        {
            serviceRating.ETicketId = null;
        }
        else
        {
            serviceRating.ETicketId = await _context.Etickets
                .Where(x => x.ETicketId == dto.ETicketId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
        }
            
            
        if (dto.ServiceId == 0)
            serviceRating.ServiceId = null;

        _context.ServiceRatings.Update(serviceRating);
        await _context.SaveChangesAsync();

        return;
    }

    public async Task DeleteAsync(int id)
    {
        var result = await _context.ServiceRatings
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new EntityNotFoundException($"ServiceRating с идентификатором: {id} не существует.");

        var user = await GetUserAsync(_currentUserService.GetUserId());

        if (result.UserId != user.Id)
        {
            throw new Domain_.Exceptions.UnauthorizedAccessException("Вы не имеете доступа к этому отзыву.");
        }

        _context.ServiceRatings.Remove(result);
        await _context.SaveChangesAsync();
    }

    private async Task<User> GetUserAsync(Guid userId)
    {
        return await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == userId)
            ?? throw new EntityNotFoundException($"User with id: {userId} does not exist.");
    }

    private async Task<List<Branch>> FilterServiceRating(ServiceRatingByCompanyQueryParametrs parametrs)
    {
        var query = _context.Branches
            .Include(s => s.Services)
                .ThenInclude(b => b.Bookings)
                    .ThenInclude(b => b.User)
            .Include(s => s.Services)
                .ThenInclude(e => e.ETickets)
                    .ThenInclude(u => u.User)
            .Include(s => s.Services)
                .ThenInclude(b => b.Bookings)
                    .ThenInclude(sr => sr.ServiceRating)
            .Include(s => s.Services)
                .ThenInclude(e => e.ETickets)
                    .ThenInclude(sr => sr.ServiceRating)
            .Where(x => x.CompanyId == parametrs.CompanyId);

        if (parametrs.BranchId.HasValue)
        {
            query = query.Where(x => x.Id == parametrs.BranchId);
        }

        query = parametrs.SortBy switch
        {
            "idDesc" => query.OrderByDescending(q => q.Id),
            "idAsc" => query.OrderBy(q => q.Id),
            _ => query
        };

        return await query.ToListAsync();
    }

    public static ServiceRatingDto MapToDto(ServiceRating rating)
    {
        if (rating == null) return null!;

        return new ServiceRatingDto(
            Id: rating.Id,
            Comment: rating.Comment,
            TicketNumber: rating.TicketNumber,
            DeskNumber: rating.DeskNumber,
            DeskName: rating.DeskName,
            SmileyRating: rating.SmileyRating,
            PredefinedTextId: rating.PredefinedTextId,
            PredefinedText: rating.PredefinedText,
            BookingId: rating.BookingId,
            Booking: rating.Booking == null ? null : new BookingDto(
                BookingId: rating.Booking.BookingId,
                BookingCode: rating.Booking.BookingCode,
                ServiceName: rating.Booking.ServiceName,
                BranchId: rating.Booking.Service?.BranchId ?? 0,
                SecondBranchId: rating.Booking.Service?.Branch?.BranchId ?? 0,
                BranchName: rating.Booking.BranchName,
                StartDate: rating.Booking.StartDate,
                StartTime: rating.Booking.StartTime.ToString()
            ),
            ETicketId: rating.ETicketId,
            ETicket: rating.ETicket == null ? null : new ETicketDto(
                ETicketId: rating.ETicket.TicketId,
                Number: rating.ETicket.Number,
                Message: rating.ETicket.Message,
                ServiceName: rating.ETicket.ServiceName,
                BranchId: rating.ETicket.Service?.BranchId ?? 0,
                SecondBranchId: rating.ETicket.Service?.Branch?.BranchId ?? 0,
                Projects: rating.ETicket.Service?.Branch?.Projects,
                BranchName: rating.ETicket.BranchName,
                null,
                ValidUntil: rating.ETicket.ValidUntil
            ),
            ServiceId: rating.ServiceId,
            Service: rating.Service == null ? null : new ServiceDto(
                Id: rating.Service.Id,
                ServiceId: rating.Service.ServiceId,
                ServiceName: rating.Service.ServiceTranslations?.FirstOrDefault()?.Name ?? "",
                Color: rating.Service.Branch.Companies.Color ?? "",
                CompanyId: rating.Service.Branch?.Companies?.Id ?? 0,
                CompanyName: rating.Service.Branch?.Companies?.Name ?? "",
                BranchId: rating.Service.Branch?.Id ?? 0,
                BranchName: rating.Service.Branch?.Name ?? "",
                CoordinateLatitude: rating.Service.Branch?.CoordinateLatitude ?? 0.0,
                CoordinateLongitude: rating.Service.Branch?.CoordinateLongitude ?? 0.0
            )
        );
    }

    private static List<ServiceRatingByCompanyDto> MapServiceRatingsToDto(List<Branch> branches)
    {
        return branches.Select(branch =>
        {
            var ratings = branch.Services
                .SelectMany(service =>
                    service.Bookings
                        .Where(b => b.ServiceRating?.IsActive == true)
                        .Select(b => new { b.ServiceRating, b.User })
                    .Concat(
                        service.ETickets
                            .Where(e => e.ServiceRating?.IsActive == true)
                            .Select(e => new { e.ServiceRating, e.User })
                    )
                )
                .Select(entry =>
                {
                    var userName = entry.User != null
                        ? $"{entry.User.LastName} {entry.User.FirstName}".Trim()
                        : "";

                    return new ServiceRatingByBranchDto(
                        entry.ServiceRating.Id,
                        entry.ServiceRating.Comment,
                        entry.ServiceRating.SmileyRating,
                        entry.ServiceRating.PredefinedTextId,
                        entry.ServiceRating.PredefinedText,
                        userName
                    );
                })
                .ToList();

            return new ServiceRatingByCompanyDto(branch.Id, branch.Name, ratings);
        }).ToList();
    }
}
