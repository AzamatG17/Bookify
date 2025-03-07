using Bookify.Application.DTOs;
using Bookify.Application.Interfaces.IServices;
using Bookify.Application.Interfaces.Services;
using Bookify.Application.Interfaces.Stores;
using Bookify.Application.Models;
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

internal sealed class BookingService(
    IApplicationDbContext context,
    IBookingStore store,
    UserManager<User> userManager,
    IBackgroundJobClient backgroundJobClient,
    IBackgroundJobService backgroundJobService,
    ICurrentUserService currentUserService) : IBookingService
{
    private readonly IApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    private readonly IBookingStore _store = store ?? throw new ArgumentNullException(nameof(store));
    private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient ?? throw new ArgumentNullException(nameof(backgroundJobClient));
    private readonly IBackgroundJobService _backgroundJobService = backgroundJobService ?? throw new ArgumentNullException(nameof(backgroundJobService));
    private readonly ICurrentUserService _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));

    public async Task<CreateBookingResponse> CreateAsync(CreateBookingRequest bookingRequest)
    {
        ArgumentNullException.ThrowIfNull(nameof(bookingRequest));

        var service = await GetServiceAsync(bookingRequest.ServiceId);

        var company = (service.Branch?.Companies)
            ?? throw new InvalidOperationException("Филиал не связан с компанией.");

        var user = await GetUserAsync(_currentUserService.GetUserId());

        bool existBooking = await _context.Bookings
            .AnyAsync(b => b.UserId == user.Id &&
                b.ServiceId == bookingRequest.ServiceId &&
                b.StartDate == bookingRequest.StartDate);

        if (existBooking)
        {
            throw new DuplicateBookingException("User has already booked a ticket for this service on the selected date.");
        }

        CreateBookingResponse response = service.Branch.Projects switch
        {
            Domain_.Enums.Projects.BookingService =>
                await _store.CreateBookingForBookingServiceAsync(
                    CreateBookingRequestModel(bookingRequest, service, user), company.BaseUrlForBookingService),

            Domain_.Enums.Projects.Onlinet =>
                await _store.CreateBookingOnlinetAsync(
                    CreateBookingRequest(bookingRequest, service, user), company.BaseUrlForOnlinet),

            _ => throw new NotSupportedException($"Неподдерживаемый тип проекта: {service.Branch.Projects}")
        };

        if (response.Success)
        {
            _backgroundJobClient.Enqueue(() => _backgroundJobService.SaveBookingAsync(response, bookingRequest, user.Id));
            
            _backgroundJobClient.Enqueue(() => _backgroundJobService.SendBookingCodeTelegram(response, user.Id, bookingRequest.Language));

        }

        return response;
    }

    public async Task DeleteAsync(GetBookingRequest request)
    {
        ArgumentNullException.ThrowIfNull(nameof(request));

        var user = await GetUserAsync(_currentUserService.GetUserId());

        var booking = await _context.Bookings
            .Include(s => s.Service)
                .ThenInclude(s => s.Branch)
                    .ThenInclude(br => br.Companies)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.BookingCode == request.BookingCode && b.User.Id == user.Id)
            ?? throw new EntityNotFoundException($"Бронирование с кодом: {request.BookingCode} не найдено.");

        ResultBooking resultBooking = booking.Service.Branch.Projects switch
        {
            Domain_.Enums.Projects.BookingService =>
                await _store.DeleteBookingForBookingServiceAsync(
                    booking.Service.Branch.Companies.BaseUrlForBookingService, booking.BookingCode, "1", booking.Language, booking.StartDate.ToString("yyyy-MM-dd")),

            Domain_.Enums.Projects.Onlinet =>
                await _store.DeleteBookingForOnlinetAsync(
                    MapToDeletBookingRequest(booking), booking.Service.Branch.Companies.BaseUrlForOnlinet),

            _ => throw new NotSupportedException($"Неподдерживаемый тип проекта: {booking.Service.Branch.Projects}")
        };

        _backgroundJobClient.Enqueue(() => _backgroundJobService.SendDeleteBookingTelegram(MapToCreateBookingResponse(booking), user.Id, request.Language));
        _backgroundJobClient.Enqueue(() => _backgroundJobService.DeleteBookingAsync(booking.Id));

        return;
    }

    private async Task<Service> GetServiceAsync(int serviceId)
    {
        return await _context.Services
            .Include(s => s.Branch)
            .ThenInclude(b => b.Companies)
            .FirstOrDefaultAsync(s => s.Id == serviceId)
            ?? throw new EntityNotFoundException($"Услуга с идентификатором: {serviceId} не найдена.");
    }

    private async Task<User> GetUserAsync(Guid userId)
    {
        return await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == userId)
            ?? throw new EntityNotFoundException($"Пользователь с идентификатором: {userId} не найден.");
    }

    private static BookingForBookingServiceRequest CreateBookingRequestModel(CreateBookingRequest request, Service service, User user)
    {
        return new BookingForBookingServiceRequest
        {
            BranchId = service.Branch.BranchId,
            Email = "test@test.uz",
            FirstName = user.FirstName,
            LastName = user.LastName,
            LanguageShortId = request.Language,
            PhoneNumber = user.UserName ?? "",
            ServiceId = service.ServiceId,
            StartDate = request.StartDate.ToUniversalTime(),
            StartTime = request.StartTime
        };
    }

    private static BookingRequest CreateBookingRequest(CreateBookingRequest request, Service service, User user)
    {
        return new BookingRequest
        {
            branchId = service.Branch.BranchId.ToString(),
            customerID = "",
            email = GenerateRandomEmail(),
            firstName = user.FirstName,
            lastName = user.LastName,
            languageShortId = request.Language,
            name = $"{user.FirstName} {user.LastName}",
            note = "",
            phoneNumber = user.PhoneNumber ?? "",
            serviceId = service.ServiceId.ToString(),
            startDate = request.StartDate.ToString("yyyyMMdd"),
            startTime = request.StartTime
        };
    }

    private static DeleteBookingRequest MapToDeletBookingRequest(Booking request)
    {
        return new DeleteBookingRequest
        {
            BookingCode = request.BookingCode,
            ClientId = request.ClientId ?? "",
            LanguageShortId = request.Language,
            StartDate = request.StartDate.ToString("yyyyMMdd")
        };
    }

    private static BookingDto MapToCreateBookingResponse(Booking b)
    {
        return new BookingDto(
            b.BookingCode,
            b.ServiceName,
            b.BranchName,
            b.StartDate,
            b.StartTime
            );
    }

    private static string GenerateRandomEmail()
    {
        return $"user{Guid.NewGuid().ToString().Substring(0, 8)}@test.uz";
    }
}
