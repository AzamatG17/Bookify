using Bookify.Application.DTOs;
using Bookify.Application.Interfaces.Services;
using Bookify.Application.Interfaces.Stores;
using Bookify.Application.Requests.Services;
using Bookify.Domain_.Exceptions;
using Bookify.Domain_.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Application.Services;

internal sealed class FreeTimeService : IFreeTimeService
{
    private readonly IApplicationDbContext _context;
    private readonly IListFreeTimesStore _listFreeTimesStore;
    private readonly IListFreeDaysStore _listFreeDaysStore;

    public FreeTimeService(IApplicationDbContext context, IListFreeTimesStore listFreeTimesStore, IListFreeDaysStore listFreeDaysStore)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _listFreeTimesStore = listFreeTimesStore ?? throw new ArgumentNullException(nameof(listFreeTimesStore));
        _listFreeDaysStore = listFreeDaysStore ?? throw new ArgumentNullException(nameof(listFreeDaysStore));
    }

    public async Task<List<FreeDayDto>> GetFreeDayListAsync(FreeTimeRequest freeTimeRequest)
    {
        ArgumentNullException.ThrowIfNull(nameof(freeTimeRequest));

        var service = await _context.Services
            .Include(b => b.Branch)
            .ThenInclude(c => c.Companies)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == freeTimeRequest.ServiceId)
            ?? throw new EntityNotFoundException($"Service with id: {freeTimeRequest.ServiceId} does not exist.");

        List<FreeDayDto> result = [];

        if (service.Branch.Projects == Domain_.Enums.Projects.BookingService)
        {
            result = await _listFreeDaysStore.GetDataBookingServiceAsync(
            service.Branch.BranchId, service.ServiceId, freeTimeRequest.DateOnly, service.Branch.Companies.BaseUrlForBookingService);
        }
        else if (service.Branch.Projects == Domain_.Enums.Projects.Onlinet)
        {
            result = await _listFreeDaysStore.GetDataOnlinetAsync(
            service.Branch.BranchId, service.ServiceId, freeTimeRequest.DateOnly, service.Branch.Companies.BaseUrlForOnlinet);
        }

        return result;
    }

    public async Task<List<FreeTimeDto>> GetFreeTimeListAsync(FreeTimeRequest freeTimeRequest)
    {
        ArgumentNullException.ThrowIfNull(nameof(freeTimeRequest));

        var service = await _context.Services
            .Include(b => b.Branch)
            .ThenInclude(c => c.Companies)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == freeTimeRequest.ServiceId)
            ?? throw new EntityNotFoundException($"Service with id: {freeTimeRequest.ServiceId} does not exist.");

        List<FreeTimeDto> result = [];

        if (service.Branch.Projects == Domain_.Enums.Projects.BookingService)
        {
            result = await _listFreeTimesStore.GetDataBookingServiceAsync(
            service.Branch.BranchId, service.ServiceId, freeTimeRequest.DateOnly, service.Branch.Companies.BaseUrlForBookingService);
        }
        else if (service.Branch.Projects == Domain_.Enums.Projects.Onlinet)
        {
            result = await _listFreeTimesStore.GetDataOnlinetAsync(
            service.Branch.BranchId, service.ServiceId, freeTimeRequest.DateOnly, service.Branch.Companies.BaseUrlForOnlinet);
        }

        return result;
    }
}
