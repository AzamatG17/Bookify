using Bookify.Application.DTOs;
using Bookify.Application.Interfaces.Services;
using Bookify.Application.Requests.Services;
using Bookify.Application.Requests.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers;

//[Authorize]
[Route("api/eticket")]
[ApiController]
public class ETicketController : ControllerBase
{
    private readonly IEticketService _service;

    public ETicketController(IEticketService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// Get Eticket status
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet("info")]
    [Authorize]
    public async Task<ActionResult> GetEticketStatusAsync([FromQuery] EticketStatusRequest request)
    {
        var result = await _service.GetETicketStatusAsync(request);

        return Ok(result);
    }

    /// <summary>
    /// Create ETicket
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ETicketDto>> CreateEticketAsync([FromBody] CreateEticketRequest request)
    {
        var response = await _service.CreateTicketAsync(request);

        return response;
    }

    [HttpDelete]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<EticketDeleteStatus>> DeleteEticketAsync([FromBody] DeleteEticketRequest request)
    {
        await _service.DeleteTicketAsync(request);

        return Ok();
    }
}
