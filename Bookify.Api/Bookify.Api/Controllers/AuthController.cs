using Bookify.Application.DTOs;
using Bookify.Application.Interfaces.IServices;
using Bookify.Application.Requests.Auth;
using Microsoft.AspNetCore.Authorization;
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
    /// Login to get JWT token.
    /// </summary>
    /// <param name="loginRequest">PhoneNumber and code to login</param>
    /// <returns></returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> LoginAsync([FromBody] LoginRequest loginRequest)
    {
        var token = await _service.LoginAsync(loginRequest);

        return Ok(token);
    }

    /// <summary>
    /// Login to get JWT token.
    /// </summary>
    /// <param name="loginRequest">PhoneNumber and code to login</param>
    /// <returns></returns>
    [HttpPost("loginTelegram")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> LoginForTelegramAsync([FromBody] LoginForTelegramRequest loginRequest)
    {
        var token = await _service.LoginForTelegramAsync(loginRequest);

        return Ok(token);
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
        await _service.RegisterAsync(request);

        return Ok();
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

    /// <summary>
    /// Send code for Telegram bot
    /// </summary>
    /// <param name="sendCodeTelegramRequest">Send code for Telegram bot</param>
    /// <returns></returns>
    [HttpPost("telegramCode")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> SendSodeTelegramAsync([FromBody] SendCodeTelegramRequest sendCodeTelegramRequest)
    {
        await _service.SendCodeForTelegramAsync(sendCodeTelegramRequest);

        return Ok();
    }

    /// <summary>
    /// Get user information
    /// </summary>
    /// <returns></returns>
    [HttpGet("user")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<UserDto> GetUserProfileAsync()
    {
        var reslut = await _service.GetUserInfoAsync();

        return reslut;
    }

    /// <summary>
    /// Get user profile for Telegram by tokenId and chatId.
    /// </summary>
    /// <param name="tokenId"></param>
    /// <returns></returns>
    [HttpGet("userDate")]
    public async Task<ActionResult<UserDto>> GetUserProfileForTelegramAsync([FromQuery] GetUserDataForTelegramRequest request)
    {
        //var userAgent = Request.Headers["User-Agent"].ToString();
        //if (!userAgent.Contains("AZAMAT Corporation", StringComparison.OrdinalIgnoreCase))
        //{
        //    throw new InvalidUserAgentException("Недопустимый User-Agent. Доступ запрещен.");
        //}

        var result = await _service.GetUserInfoWithChatIdAndTokenId(request.chatId, request.tokenId);

        return Ok(result);
    }
}
