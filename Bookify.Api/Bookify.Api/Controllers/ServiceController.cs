using Bookify.Application.DTOs;
using Bookify.Application.Interfaces.Services;
using Bookify.Application.QueryParameters;
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

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<CompanyWithBranchesDto>>> GetAllServiceAsync([FromQuery] ServiceQueryParameters service)
    {
        var companies = await _service.GetAllAsync(service);

        return Ok(companies);
    }
}
