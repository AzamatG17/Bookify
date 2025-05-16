using Bookify.Application.DTOs;
using Bookify.Application.Interfaces.IServices;
using Bookify.Application.Interfaces.Services;
using Bookify.Application.Interfaces.Stores;
using Bookify.Application.Requests.Services;
using Bookify.Application.Requests.Stores;
using Bookify.Application.Responses;
using Bookify.Domain_.Entities;
using Bookify.Domain_.Enums;
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
        ArgumentNullException.ThrowIfNull(request);

        var eTicket = await _context.Etickets
            .Include(u => u.User)
            .Include(s => s.Service)
                .ThenInclude(b => b.Branch)
                    .ThenInclude(c => c.Companies)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.TicketId == request.TicketId && e.Service.Branch.BranchId == request.BranchId)
            ?? throw new EntityNotFoundException($"ETicket с идентификатором: {request.TicketId} не найден в филиале: {request.BranchId}");

        var user = await GetUserAsync(_currentUserService.GetUserId());

        if (eTicket.User.Id != user.Id)
        {
            throw new Domain_.Exceptions.UnauthorizedAccessException("You do not have permission to access this eTicket.");
        }

        object result = eTicket.Service.Branch.Projects switch
        {
            Domain_.Enums.Projects.BookingService =>
                await _store.GetEtickertStatusBookingServiceAsync(request, eTicket.Service.Branch.Companies.BaseUrlForBookingService),

            _ => throw new NotSupportedException($"Тип проекта не поддерживается: {eTicket.Service.Branch.Projects}")
        };

        return result;
    }
    
    public async Task<ETicketDto> CreateTicketAsync(CreateEticketRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var service = await GetServiceAsync(request.ServiceId);

        var company = (service.Branch?.Companies)
            ?? throw new InvalidOperationException("Филиал не связан с компанией.");

        var user = await GetUserAsync(_currentUserService.GetUserId());

        var now = DateTime.UtcNow.AddHours(5);
        var currentDay = (int)now.DayOfWeek;
        var currentTime = now.TimeOfDay;

        var openingHours = service.Branch.OpeningTimeBranches
            .FirstOrDefault(o => o.Day == currentDay);

        if (openingHours == null || currentTime < openingHours.StartTime || currentTime > openingHours.EndTime)
        {
            throw new InvalidOperationException("Подача заявки на eTicket вне рабочего времени филиала.");
        }

        bool existBooking = await _context.Etickets
            .AnyAsync(b => b.UserId == user.Id &&
                b.IsActive &&
                b.CreatedAtUtc.Date == DateTime.UtcNow.Date);

        if (existBooking)
        {
            throw new DuplicateBookingException("Пользователь уже забронировал eTicket для этого сервиса на выбранную дату.");
        }

        EticketResponse response = service.Branch.Projects switch
        {
            Domain_.Enums.Projects.BookingService =>
                await _store.CreateTicketForBookingServiceAsync(
                    CreateEticketRequestModel(request, service, user), company.BaseUrlForBookingService),

            Domain_.Enums.Projects.Onlinet =>
                await _store.CreateTicketForOnlinetAsync(
                    CreateETicketOnlinetModel(request, service, user), company.BaseUrlForOnlinet),

            _ => throw new NotSupportedException($"Тип проекта не поддерживается: {service.Branch.Projects}")
        };

        if (response.Success)
        {
            DateTime dateTime = DateTime.Now;

            _backgroundJobClient.Enqueue(() => _backgroundJobService.SaveETicketAsync(response, request, user.Id, dateTime));
            
            _backgroundJobClient.Enqueue(() => _backgroundJobService.SendETicketTelegram(response, user.Id, request.Language, dateTime));
        }

        return MapToETicketDto(response, service.Branch.Projects);
    }

    public async Task<DeleteResponse> DeleteTicketAsync(DeleteEticketRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var user = await GetUserAsync(_currentUserService.GetUserId());

        var eTicket = await _context.Etickets
            .Include(s => s.Service)
                .ThenInclude(s => s.Branch)
                    .ThenInclude(br => br.Companies)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.ETicketId == request.eTicketId && e.UserId == user.Id)
            ?? throw new EntityNotFoundException($"ETicket с номером: {request.eTicketId} не найден.");

        string onlyNumber = new string(eTicket.Number.Where(char.IsDigit).ToArray());

        DeleteResponse deleteResponse = eTicket.Service.Branch.Projects switch
        {
            Projects.BookingService =>
                await _store.DeleteBookingServiceAsync(eTicket.Service.Branch.Companies.BaseUrlForBookingService, eTicket.Service.Branch.BranchId, eTicket.Number),

            Projects.Onlinet =>
                await _store.DeleteOnlinetAsync(eTicket.Service.Branch.Companies.BaseUrlForOnlinet, user.Id.ToString(), onlyNumber)
        };

        //_backgroundJobClient.Enqueue(() => _backgroundJobService.DeleteEticketAsync(eTicket.Id));
        _backgroundJobClient.Enqueue(() => _backgroundJobService.SendDeleteETicketTelegram(MapToETicketResponse(eTicket), user.Id, request.Language));

        await _backgroundJobService.DeleteEticketAsync(eTicket.Id);

        return deleteResponse;
    }

    private async Task<Service> GetServiceAsync(int serviceId)
    {
        return await _context.Services
            .Include(s => s.Branch)
                .ThenInclude(b => b.Companies)
            .Include(s => s.Branch)
                .ThenInclude(o => o.OpeningTimeBranches)
            .FirstOrDefaultAsync(s => s.Id == serviceId)
            ?? throw new EntityNotFoundException($"Сервис с идентификатором: {serviceId} не найден.");
    }

    private async Task<User> GetUserAsync(Guid userId)
    {
        return await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == userId)
            ?? throw new EntityNotFoundException($"Пользователь с идентификатором: {userId} не найден.");
    }

    private static EticketRequest CreateEticketRequestModel(CreateEticketRequest request, Service service, User user)
    {
        return new EticketRequest
        {
            BranchId = service.Branch.BranchId,
            DeviceType = 3,
            LanguageId = request.Language,
            PhoneNumber = user.UserName ?? "",
            ServiceId = service.ServiceId,
        };
    }

    private static ETicketOnlinetRequest CreateETicketOnlinetModel(CreateEticketRequest request, Service service, User user)
    {
        return new ETicketOnlinetRequest
        {
            branchId = service.Branch.BranchId.ToString(),
            deviceType = 3,
            languageId = request.Language,
            phoneNumber = user.UserName,
            serviceId = service.ServiceId,
            deviceId = user.Id.ToString(),
        };
    }

    private static ETicketDto MapToETicketDto(EticketResponse e, Projects projects)
    {
        return new ETicketDto(
            e.TicketId,
            e.Number,
            e.Message,
            e.Service,
            0,
            e.BranchId,
            projects,
            e.BranchName,
            e.BranchAddress,
            e.ValidUntil
            );
    }

    private static EticketResponse MapToETicketResponse(ETicket e)
    {
        return new EticketResponse
        {
            Number = e.Number,
            Service = e.ServiceName,
            BranchAddress = e.Service?.Branch?.BranchAddres ?? "",
            BranchName = e.BranchName,
            CreatedTime = e.CreatedTime.ToString()
        };
    }
}
