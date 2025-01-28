using Bookify.Application.DTOs;
using Bookify.Application.Interfaces.Services;
using Bookify.Application.Requests.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers;

//[Authorize]
[Route("api/availableTime")]
[ApiController]
public class AvailableTimeController : ControllerBase
{
    private readonly IFreeTimeService _service;

    public AvailableTimeController(IFreeTimeService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<List<FreeDayDto>> GetFreeDaysAsync([FromQuery] FreeTimeRequest freeTimeRequest)
    {
        var reslut = await _service.GetFreeDayListAsync(freeTimeRequest);

        return reslut;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<List<FreeTimeDto>> GetFreeTimesAsync([FromQuery] FreeTimeRequest freeTimeRequest)
    {
        var reslut = await _service.GetFreeTimeListAsync(freeTimeRequest);

        return reslut;
    }
}
