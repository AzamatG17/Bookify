using Bookify.Application.DTOs;
using Bookify.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers;

[Route("serviceRating")]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ServiceRatingDto>>> GetAllServiceRatingAsync()
    {
        var result = await _ratingService.GetAllServiceRatingAsync();

        return Ok(result);
    }

    /// <summary>
    /// Create a new ServiceRating.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ServiceRatingDto>> CreateServiceRatingAsync([FromBody] ServiceRatingForCreateDto serviceRatingForCreateDto)
    {
        var response = await _ratingService.CreateServiceRatingAsync(serviceRatingForCreateDto);

        return Ok(response);
    }
}
