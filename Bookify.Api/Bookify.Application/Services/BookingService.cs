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

        var bookingRequestModel = CreateBookingRequestModel(bookingRequest, service, user);

        CreateBookingResponse response = company.Projects switch
        {
            Domain_.Enums.Projects.BookingService => await _store.CreateBookingForBookingServiceAsync(bookingRequestModel, company.BaseUrl),
            //Domain_.Enums.Projects.Onlinet => await _store.CreateBookingOnlinetAsync(bookingRequestModel, company.BaseUrl),
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

    //private static BookingRequest CreateBookingRequestOnlinetModel(CreateBookingRequest request, Service service, User user)
    //{
    //    return new BookingRequest
    //    {
    //        BranchId = service.Branch.Id.ToString(),
    //        ServiceId = service.Id.ToString(),
    //        CustomerID = user.Id.ToString() ?? "",
    //        Email = "test@gmail.com",
    //        FirstName = user.FirstName,
    //        LastName = user.LastName,
    //        Name = $"{user.LastName} {user.FirstName}",
    //        Note = "",
    //        PhoneNumber = user.PhoneNumber ?? "",
    //        LanguageShortId = request.Language,
    //        StartDate = request.StartDate,
    //        StartTime = request.StartTime
    //    };
    //}

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

}
