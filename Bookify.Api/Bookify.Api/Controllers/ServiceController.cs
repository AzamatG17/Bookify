using Bookify.Application.DTOs;
using Bookify.Application.Interfaces.Services;
using Bookify.Application.QueryParameters;
using Bookify.Application.Requests.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers;

//[Authorize]
[Route("api/service")]
[ApiController]
public class ServiceController : ControllerBase
{
    private readonly IServicesService _service;

    public ServiceController(IServicesService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// Get all Services
    /// </summary>
    /// <param name="service"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ServiceWithRatingDto>>> GetAllServiceAsync([FromQuery] ServiceQueryParameters service)
    {
        var companies = await _service.GetAllAsync(service);

        return Ok(companies);
    }

    /// <summary>
    /// Update a Service
    /// </summary>
    /// <returns></returns>
    [HttpPut("{id:int:min(1)}")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateServiceAsync([FromRoute] int id, [FromBody] UpdateServiceGroupRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest($"Route parameter does not match with body parameter: {request.Id}");
        }

        await _service.UpdateServiceGroupAsync(request);

        return Ok();
    }
}
