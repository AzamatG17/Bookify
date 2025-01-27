using Bookify.Application.Interfaces.IServices;
using Bookify.Application.Interfaces.Services;
using Bookify.Application.Requests.Auth;
using Bookify.Application.Requests.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers;

[Route("api/updateData")]
[ApiController]
public class UpdateDataController : ControllerBase
{
    private readonly IBranchService _branchService;
    private readonly IServicesService _services;
    private readonly IAuthService _authService;

    public UpdateDataController(IBranchService branchService, IServicesService services, IAuthService authService)
    {
        _branchService = branchService ?? throw new ArgumentNullException(nameof(branchService));
        _services = services ?? throw new ArgumentNullException(nameof(services));
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    /// <summary>
    /// Login to get JWT token.
    /// </summary>
    /// <param name="loginRequest">login and password to Login</param>
    /// <returns></returns>
    [HttpPost("loginAdmin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> LoginAsync([FromBody] LoginForAdminRequest loginRequest)
    {
        var token = await _authService.LoginForAdminAsync(loginRequest);

        return Ok(token);
    }

    /// <summary>
    /// Update data for Branch table
    /// </summary>
    /// <param name="request">The Company Id</param>
    /// <returns></returns>
    [HttpGet("{Id:int:min(1)}/branch", Name = nameof(UpdateBranchesByCompanyIdAsync))]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateBranchesByCompanyIdAsync([FromRoute] CompanyRequest request)
    {
        var result = await _branchService.UpdateDateAsync(request);

        return Ok(result);
    }

    /// <summary>
    /// Update data for Service table
    /// </summary>
    /// <param name="companyRequest">The Company Id</param>
    /// <returns></returns>
    [HttpGet("{BranchId:int:min(1)}/service", Name = nameof(UpdateServiceByBranchIdAsync))]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateServiceByBranchIdAsync([FromRoute] BranchRequest branchRequest)
    {
        var result = await _services.UpdateDataAsync(branchRequest);

        return Ok(result);
    }
}
