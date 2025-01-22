using Bookify.Application.DTOs;
using Bookify.Application.Interfaces.IServices;
using Bookify.Application.Models;
using Bookify.Application.Requests.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers;

//[Authorize]
[Route("api/branch")]
[ApiController]
public class BranchController : ControllerBase
{
    private readonly IBranchService _service;

    public BranchController(IBranchService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<CompanyWithBranchesDto>>> GetBranchAllAsync()
    {
        var companies = await _service.GetAllAsync();

        return Ok(companies);
    }

    /// <summary>
    /// Gets branch by Company Id.
    /// </summary>
    /// <param name="request">The Branch Id</param>
    /// <returns></returns>
    [HttpGet("{id:int:min(1)}/branch", Name = nameof(GetBranchByIdAsync))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CompanyWithBranchesDto>> GetBranchByIdAsync([FromRoute] CompanyRequest request)
    {
        var company = await _service.GetByIdAsync(request);

        return Ok(company);
    }
}
