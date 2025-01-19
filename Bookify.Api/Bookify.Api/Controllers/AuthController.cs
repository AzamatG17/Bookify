using Bookify.Application.Interfaces;
using Bookify.Application.Requests.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service)); 
    }

    /// <summary>
    /// Register to create a new user.
    /// </summary>
    /// <param name="request">Register to create a new user.</param>
    /// <returns></returns>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> RegisterAsync([FromBody] RegisterRequest request)
    {
        var token = await _service.RegisterAsync(request);

        return Ok(token);
    }

    /// <summary>
    /// Send sms for confirm PhoneNumber
    /// </summary>
    /// <param name="sendSmsCodeRequest">Send sms for confirm PhoneNumber</param>
    /// <returns></returns>
    [HttpPost("sms")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> SendConfirmCodeAsync([FromBody] SendSmsCodeRequest sendSmsCodeRequest)
    {
        await _service.SendSmsCodeAsync(sendSmsCodeRequest);

        return Ok();
    }
}
