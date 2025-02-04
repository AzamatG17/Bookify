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
using Microsoft.Extensions.DependencyInjection;

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
            ?? throw new InvalidOperationException("Branch is not associated with a company.");

        var user = await GetUserAsync(_currentUserService.GetUserId());

        CreateBookingResponse response = company.Projects switch
        {
            Domain_.Enums.Projects.BookingService => 
                await _store.CreateBookingForBookingServiceAsync(
                    CreateBookingRequestModel(bookingRequest, service, user), company.BaseUrl),

            Domain_.Enums.Projects.Onlinet => 
                await _store.CreateBookingOnlinetAsync(
                    CreateBookingRequest(bookingRequest, service, user), company.BaseUrl),

            _ => throw new NotSupportedException($"Unsupported project type: {company.Projects}")
        };

        if (response.Success)
        {
            _backgroundJobClient.Enqueue(() => _backgroundJobService.SaveBookingAsync(response, bookingRequest, user.Id));
        }

        return response;
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

    private static BookingForBookingServiceRequest CreateBookingRequestModel(CreateBookingRequest request, Service service, User user)
    {
        return new BookingForBookingServiceRequest
        {
            BranchId = service.Branch.BranchId,
            Email = "test@test.uz",
            FirstName = user.FirstName,
            LastName = user.LastName,
            LanguageShortId = request.Language,
            PhoneNumber = user.PhoneNumber ?? "",
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

    private static string GenerateRandomEmail()
    {
        return $"user{Guid.NewGuid().ToString().Substring(0, 8)}@test.uz";
    }
}
