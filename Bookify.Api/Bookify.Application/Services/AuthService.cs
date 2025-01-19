using Bookify.Application.Constants;
using Bookify.Application.Interfaces;
using Bookify.Application.Models;
using Bookify.Application.Requests.Auth;
using Bookify.Domain_.Entities;
using Bookify.Domain_.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Application.Services;

internal sealed class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenHandler _jwtTokenHandler;
    private readonly ISmsCodeService _smsCodeService;
    private readonly ISmsService _smsService;

    public AuthService(UserManager<User> userManager, IJwtTokenHandler jwtTokenHandler, ISmsCodeService smsCodeService, ISmsService smsService)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _jwtTokenHandler = jwtTokenHandler ?? throw new ArgumentNullException(nameof(jwtTokenHandler));
        _smsCodeService = smsCodeService ?? throw new ArgumentNullException(nameof(smsCodeService));
        _smsService = smsService ?? throw new ArgumentNullException(nameof(smsService));
    }

    public Task<string> LoginAsync(LoginRequest loginRequest)
    {
        throw new NotImplementedException();
    }

    public async Task<string> RegisterAsync(RegisterRequest request)
    {
        ArgumentNullException.ThrowIfNull(nameof(request));

        var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == request.PhoneNumber);
        if (existingUser is not null)
        {
            throw new UserNameAlreadyTakenException($"Phone Number: {request.PhoneNumber} is already taken.");
        }

        //if (!_smsCodeService.ValidateCode(request.PhoneNumber, request.SmsCode))
        //{
        //    throw new SmsCodeValidationException("The SMS code is invalid or expired.");
        //}

        var newUser = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.PhoneNumber,
            Email = string.IsNullOrEmpty(request.Email) ? null : request.Email,
            BirthDate = request.BirthDate,
            Gender = request.Gender ?? Domain_.Enums.Gender.Other,
        };

        var createResult = await _userManager.CreateAsync(newUser);
        if (!createResult.Succeeded)
        {
            var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"User creation failed: {errors}");
        }

        var addToRoleResult = await _userManager.AddToRoleAsync(newUser, RoleConsts.User);
        if (!addToRoleResult.Succeeded)
        {
            var errors = string.Join("; ", addToRoleResult.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to assign role 'User': {errors}");
        }

        var roles = await _userManager.GetRolesAsync(newUser);

        var token = _jwtTokenHandler.GenerateToken(newUser, roles);

        return token;
    }

    public async Task SendSmsCodeAsync(SendSmsCodeRequest sendSmsCodeRequest)
    {
        ArgumentNullException.ThrowIfNull(nameof(sendSmsCodeRequest));

        var smsCode = new Random().Next(1000, 9999).ToString();
        _smsCodeService.SaveCode(sendSmsCodeRequest.PhoneNumber, smsCode);

        var messsage = new SmsMessage(sendSmsCodeRequest.PhoneNumber, "Это тест от Eskiz");
        await _smsService.SendMessage(messsage);
    }
}
