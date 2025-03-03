using Bookify.Application.DTOs;
using Bookify.Application.Interfaces.Services;
using Bookify.Application.Requests.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Bookify.Api.Controllers;

//[Authorize]
[Route("api/company")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly ICompaniesService _service;

    public CompaniesController(ICompaniesService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// Gets a list of Companies.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [HttpHead]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<CompaniesDto>>> GetCompanyAllAsync()
    {
        var companies = await _service.GetAllAsync();

        return Ok(companies);
    }

    /// <summary>
    /// Get a list of Comapnies for Admin
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = "Admin")]
    [HttpGet("info")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<CompaniesForAdminDto>>> GetCompanyAllForAdminAsync()
    {
        var companies = await _service.GetAllForAdminAsync();

        return Ok(companies);
    }

    /// <summary>
    /// Gets company by Id.
    /// </summary>
    /// <param name="request">The Company Id</param>
    /// <returns></returns>
    [HttpGet("{Id:int:min(1)}/company", Name = nameof(GetCompanyByIdAsync))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CompaniesDto>> GetCompanyByIdAsync([FromRoute] CompanyRequest request)
    {
        var company = await _service.GetByIdAsync(request);

        return Ok(company);
    }

    /// <summary>
    /// Create a new Company.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CompaniesDto>> CreateCompanyAsync([FromBody] CreateCompanyRequest request)
    {
        var response = await _service.CreateAsync(request);

        return CreatedAtAction(nameof(GetCompanyByIdAsync), new { id = response.Id }, response);
    }

    /// <summary>
    /// Update Company.
    /// </summary>
    /// <param name="id">Company Id</param>
    /// <param name="request">Company to update</param>
    /// <returns></returns>
    [HttpPut("{id:int:min(1)}")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateCompanyAsync([FromRoute] int id, [FromBody] UpdateCompanyRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest($"Route parameter does not match with body parameter: {request.Id}");
        }

        await _service.UpdateAsync(request);

        return NoContent();
    }

    /// <summary>
    /// Delete a single Company.
    /// </summary>
    /// <param name="request">Company od to delete</param>
    /// <returns></returns>
    [HttpDelete("{Id:int:min(1)}")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteCompanyAsync([FromRoute] CompanyRequest request)
    {
        await _service.DeleteAsync(request);

        return NoContent();
    }

    /// <summary>
    /// Gets allowed methods for this resource.
    /// </summary>
    /// <returns>Allowed methods for this resource</returns>
    [HttpOptions]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetOptions()
    {
        string[] options = ["HEAD", "GET", "POST", "PUT", "DELETE"];

        HttpContext.Response.Headers.Append("X-Options", JsonConvert.SerializeObject(options));

        return Ok();
    }
}
