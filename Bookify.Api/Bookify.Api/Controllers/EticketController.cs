using Bookify.Application.DTOs;
using Bookify.Application.Interfaces.Services;
using Bookify.Application.Requests.Services;
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
}
