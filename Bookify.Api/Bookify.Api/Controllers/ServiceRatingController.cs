using Bookify.Application.DTOs;
using Bookify.Application.Interfaces.Services;
using Bookify.Application.QueryParameters;
using Bookify.Application.Requests.Services;
using Bookify.Domain_.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers;

[Route("api/serviceRating")]
[ApiController]
public class ServiceRatingController : ControllerBase
{
    private readonly IServiceRatingService _ratingService;

    public ServiceRatingController(IServiceRatingService ratingService)
    {
        _ratingService = ratingService ?? throw new ArgumentNullException(nameof(ratingService));
    }

    /// <summary>
    /// Get all ServiceRating
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ServiceRatingDto>>> GetAllServiceRatingAsync()
    {
        var result = await _ratingService.GetAllServiceRatingAsync();

        return Ok(result);
    }

    [HttpGet("GetByCompanyId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetAllServiceRatingByCompany([FromQuery] ServiceRatingByCompanyQueryParametrs queryParametrs)
    {
        var result = await _ratingService.GetAllServiceRatingByCompanyAsync(queryParametrs);

        return Ok(result);
    }

    /// <summary>
    /// Get ServiceRating
    /// </summary>
    /// <returns></returns>
    [HttpGet("getinfo")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ServiceRatingDto>>> GetServiceRatingAsync()
    {
        var result = await _ratingService.GetServiceRatingAsync();

        return Ok(result);
    }

    /// <summary>
    /// Create a new ServiceRating.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ServiceRatingDto>> CreateServiceRatingAsync([FromBody] ServiceRatingForCreateDto serviceRatingForCreateDto)
    {
        var response = await _ratingService.CreateServiceRatingAsync(serviceRatingForCreateDto);

        return Ok(response);
    }

    /// <summary>
    /// Update ServiceRating.
    /// </summary>
    /// <param name="id">ServiceRating Id</param>
    /// <param name="request">ServiceRating to update</param>
    /// <returns></returns>
    [HttpPut("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateServiceRatingAsync([FromRoute] int id, [FromBody] ServiceRatingForUpdateDto request)
    {
        if (id != request.Id)
        {
            return BadRequest($"Route parameter does not match with body parameter: {request.Id}");
        }

        await _ratingService.UpdateAsync(request);

        return NoContent();
    }

    /// <summary>
    /// Delete a ServiceRating.
    /// </summary>
    /// <param name="request">Company od to delete</param>
    /// <returns></returns>
    [HttpDelete("{id:int:min(1)}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteServiceRatingAsync([FromRoute] int id)
    {
        await _ratingService.DeleteAsync(id);

        return NoContent();
    }
}
