using Bookify.Application.DTOs;
using Bookify.Application.Interfaces.Services;
using Bookify.Application.Requests.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers;

[Route("api/predefinedText")]
public class PredefinedTextController : ControllerBase
{
    private readonly IPredefinedTextService _service;

    public PredefinedTextController(IPredefinedTextService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// Get all PredefinedText
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<PredefinedTextDto>>> GetAllPredefinedTextAsync()
    {
        var result = await _service.GetAllAsync();

        return Ok(result);
    }

    /// <summary>
    /// Create a new PredefinedText.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PredefinedTextDto>> CreatePredefinedTextAsync([FromBody] PredefinedTextForCreateDto predefinedTextForCreateDto)
    {
        var response = await _service.CreateAsync(predefinedTextForCreateDto);

        return Ok(response);
    }

    /// <summary>
    /// Update a PredefinedText.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="predefinedTextDto"></param>
    /// <returns></returns>
    [HttpPut("{id:int:min(1)}")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdatePredefinedTextAsync([FromRoute] int id, [FromBody] PredefinedTextDto predefinedTextDto)
    {
        if (id != predefinedTextDto.Id)
        {
            return BadRequest($"Route parameter does not match with body parameter: {predefinedTextDto.Id}");
        }

        await _service.UpdateAsync(predefinedTextDto);

        return NoContent();
    }

    /// <summary>
    /// Delete a single PredefinedText.
    /// </summary>
    /// <param name="request">PredefinedText od to delete</param>
    /// <returns></returns>
    [HttpDelete("{Id:int:min(1)}")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteCompanyAsync([FromRoute] PredefinedTextRequest request)
    {
        await _service.DeleteAsync(request);

        return NoContent();
    }
}
