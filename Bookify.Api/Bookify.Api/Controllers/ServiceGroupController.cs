using Bookify.Application.DTOs;
using Bookify.Application.Interfaces.Services;
using Bookify.Application.QueryParameters;
using Bookify.Application.Requests.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers;

[ApiController]
[Route("api/serviceGroup")]
public class ServiceGroupController : ControllerBase
{
    private readonly IServiceGroupService _service;
    public ServiceGroupController(IServiceGroupService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// Get all service groups
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ServiceGroupDto>>> GetServiceGroupAllAsync([FromQuery] ServiceGroupQueryParameters parameters)
    {
        var serviceGroups = await _service.GetAllAsync(parameters);

        return Ok(serviceGroups);
    }

    /// <summary>
    /// Get all service groups for admin
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Policy = "Admin")]
    [HttpGet("admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ServiceGroupDto>>> GetServiceGroupForAdminAsync([FromQuery] ServiceGroupQueryParameters parameters)
    {
        var serviceGroups = await _service.GetAllForAdminAsync(parameters);

        return Ok(serviceGroups);
    }

    /// <summary>
    /// Create a new service group
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ServiceGroupDto>> CreateServiceGroupAsync([FromBody] ServiceGroupRequest request)
    {
        var response = await _service.CreateAsync(request);

        return CreatedAtAction(nameof(GetServiceGroupAllAsync), new { id = response.Id }, response);
    }

    /// <summary>
    /// Update an existing service group
    /// </summary>
    /// <param name="id"></param>
    /// <param name="serviceGroup"></param>
    /// <returns></returns>
    [HttpPut("{id:int:min(1)}")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ServiceGroupDto>> UpdateServiceGroupAsync([FromRoute] int id, [FromBody] ServicegroupUpdateDto serviceGroup)
    {
        if (id != serviceGroup.Id)
        {
            return BadRequest($"Route parameter does not match with body parameter: {serviceGroup.Id}");
        }

        var response = await _service.UpdateAsync(serviceGroup);

        return Ok(response);
    }

    /// <summary>
    /// Delete a service group
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int:min(1)}")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteServiceGroupAsync([FromRoute] int id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }
}
