using Bookify.Application.DTOs;
using Bookify.Application.Interfaces.IServices;
using Bookify.Application.Interfaces.Services;
using Bookify.Application.Interfaces.Stores;
using Bookify.Application.Requests.Services;
using Bookify.Application.Requests.Stores;
using Bookify.Application.Responses;
using Bookify.Domain_.Entities;
using Bookify.Domain_.Exceptions;
using Bookify.Domain_.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Application.Services;

internal sealed class EticketService : IEticketService
{
    private readonly IApplicationDbContext _context;
    private readonly IEticketStore _store;
    private readonly UserManager<User> _userManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IBackgroundJobService _backgroundJobService;
    private readonly IBackgroundJobClient _backgroundJobClient;

    public EticketService(
        IApplicationDbContext context,
        IEticketStore ticketStore,
        UserManager<User> userManager,
        ICurrentUserService currentUserService,
        IBackgroundJobService backgroundJobService,
        IBackgroundJobClient backgroundJobClient)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _store = ticketStore ?? throw new ArgumentNullException(nameof(ticketStore));
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _backgroundJobService = backgroundJobService ?? throw new ArgumentNullException(nameof(backgroundJobService));
        _backgroundJobClient = backgroundJobClient ?? throw new ArgumentNullException(nameof(backgroundJobClient));
    }

    public async Task<object> GetETicketStatusAsync(EticketStatusRequest request)
    {
        ArgumentNullException.ThrowIfNull(nameof(request));

        var eTicket = await _context.Etickets
            .Include(u => u.User)
            .Include(s => s.Service)
            .ThenInclude(b => b.Branch)
            .ThenInclude(c => c.Companies)
            .FirstOrDefaultAsync(e => e.TicketId == request.TicketId && e.Service.Branch.BranchId == request.BranchId)
            ?? throw new EntityNotFoundException($"ETicket with TickedId: {request.TicketId} does not exist in Branch: {request.BranchId}");

        var user = await GetUserAsync(_currentUserService.GetUserId());

        if (eTicket.User.Id != user.Id)
        {
            throw new Domain_.Exceptions.UnauthorizedAccessException("You do not have permission to access this eTicket.");
        }

        object result = eTicket.Service.Branch.Companies.Projects switch
        {
            Domain_.Enums.Projects.BookingService =>
                await _store.GetEtickertStatusBookingServiceAsync(request, eTicket.Service.Branch.Companies.BaseUrl),

            _ => throw new NotSupportedException($"Unsupported project type: {eTicket.Service.Branch.Companies.Projects}")
        };

        return result;
    }

    public async Task<ETicketDto> CreateTicketAsync(CreateEticketRequest request)
    {
        ArgumentNullException.ThrowIfNull(nameof(request));

        var service = await GetServiceAsync(request.ServiceId);

        var company = (service.Branch?.Companies)
            ?? throw new InvalidOperationException("Branch is not associated with a company.");

        var user = await GetUserAsync(_currentUserService.GetUserId());

        bool existBooking = await _context.Etickets
            .AnyAsync(b => b.UserId == user.Id &&
                b.ServiceId == request.ServiceId &&
                b.CreatedAtUtc.Date == DateTime.UtcNow.Date);

        if (existBooking)
        {
            throw new DuplicateBookingException("User has already booked a ETicket for this service on the selected date.");
        }

        EticketResponse response = company.Projects switch
        {
            Domain_.Enums.Projects.BookingService =>
                await _store.CreateTicketForBookingServiceAsync(
                    CreateEticketRequestModel(request, service, user), company.BaseUrl),

            Domain_.Enums.Projects.Onlinet =>
                await _store.CreateTicketForOnlinetAsync(
                    CreateETicketOnlinetModel(request, service, user), company.BaseUrl),

            _ => throw new NotSupportedException($"Unsupported project type: {company.Projects}")
        };

        if (response.Success)
        {
            _backgroundJobClient.Enqueue(() => _backgroundJobService.SaveETicketAsync(response, request, user.Id));
        }

        return MapToETicketDto(response);
    }

    public async Task DeleteTicketAsync()
    {

    }

    private async Task<Service> GetServiceAsync(int serviceId)
    {
        return await _context.Services
            .Include(s => s.Branch)
            .ThenInclude(b => b.Companies)
            .FirstOrDefaultAsync(s => s.Id == serviceId)
            ?? throw new EntityNotFoundException($"Service with id: {serviceId} does not exist.");
    }

    private async Task<User> GetUserAsync(Guid userId)
    {
        return await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == userId)
            ?? throw new EntityNotFoundException($"User with id: {userId} does not exist.");
    }

    private static EticketRequest CreateEticketRequestModel(CreateEticketRequest request, Service service, User user)
    {
        return new EticketRequest
        {
            BranchId = service.Branch.BranchId,
            DeviceType = 3,
            LanguageId = request.Language,
            PhoneNumber = user.PhoneNumber ?? "",
            ServiceId = service.ServiceId,
        };
    }

    private static ETicketOnlinetRequest CreateETicketOnlinetModel(CreateEticketRequest request, Service service, User user)
    {
        return new ETicketOnlinetRequest
        {
            branchId = service.Branch.BranchId,
            deviceType = 3,
            languageId = request.Language,
            phoneNumber = "+998998907641",
            serviceId = service.ServiceId,
            deviceId = "sz7plvmtk8dxv9rdgqmjvt",
        };
    }

    private static ETicketDto MapToETicketDto(EticketResponse e)
    {
        return new ETicketDto(
            e.TicketId,
            e.Number,
            e.Message,
            e.Service,
            e.BranchId,
            e.BranchName,
            e.BranchAddress,
            e.ValidUntil
            );
    }
}
