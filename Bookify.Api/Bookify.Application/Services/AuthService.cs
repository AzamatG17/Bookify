using Bookify.Application.Constants;
using Bookify.Application.DTOs;
using Bookify.Application.Interfaces;
using Bookify.Application.Interfaces.IServices;
using Bookify.Application.Models;
using Bookify.Application.Requests.Auth;
using Bookify.Domain_.Entities;
using Bookify.Domain_.Exceptions;
using Bookify.Domain_.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Application.Services;

internal sealed class AuthService : IAuthService
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenHandler _jwtTokenHandler;
    private readonly ISmsCodeService _smsCodeService;
    private readonly ISmsService _smsService;
    private readonly ITelegramService _telegramService;
    private readonly ICurrentUserService _currentUserService;

    public AuthService(
        IApplicationDbContext context,
        UserManager<User> userManager,
        IJwtTokenHandler jwtTokenHandler, 
        ISmsCodeService smsCodeService,
        ISmsService smsService,
        ITelegramService telegramService,
        ICurrentUserService currentUserService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _jwtTokenHandler = jwtTokenHandler ?? throw new ArgumentNullException(nameof(jwtTokenHandler));
        _smsCodeService = smsCodeService ?? throw new ArgumentNullException(nameof(smsCodeService));
        _smsService = smsService ?? throw new ArgumentNullException(nameof(smsService));
        _telegramService = telegramService ?? throw new ArgumentNullException(nameof(telegramService));
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
    }

    public async Task<string> LoginAsync(Requests.Auth.LoginRequest loginRequest)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(nameof(loginRequest));

        var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginRequest.PhoneNumber)
            ?? throw new UserNameNotExistException($"Phone Number: {loginRequest.PhoneNumber} does not exist.");

        if (!_smsCodeService.ValidateCode(loginRequest.PhoneNumber, loginRequest.Code))
        {
            throw new SmsCodeValidationException("The code is invalid or expired. Retry again.");
        }

        var roles = await _userManager.GetRolesAsync(existingUser);

        var token = _jwtTokenHandler.GenerateToken(existingUser, roles);

        return token;
    }

    public async Task<string> LoginForTelegramAsync(LoginForTelegramRequest loginForTelegramRequest)
    {
        ArgumentNullException.ThrowIfNull(nameof(loginForTelegramRequest));

        var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginForTelegramRequest.PhoneNumber)
            ?? throw new UserNameNotExistException($"Phone Number: {loginForTelegramRequest.PhoneNumber} does not exist.");

        if (existingUser.ChatId != loginForTelegramRequest.ChatId)
        {
            throw new ChatIdValidationException("The provided ChatId is invalid for the given phone number.");
        }

        var roles = await _userManager.GetRolesAsync(existingUser);

        var token = _jwtTokenHandler.GenerateToken(existingUser, roles);

        return token;
    }

    public async Task RegisterAsync(Requests.Auth.RegisterRequest request)
    {
        ArgumentNullException.ThrowIfNull(nameof(request));

        var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == request.PhoneNumber);
        if (existingUser is not null)
        {
            existingUser.PhoneNumber = request.PhoneNumber;
            existingUser.FirstName = request.FirstName;
            existingUser.LastName = request.LastName;
            existingUser.ChatId = request.ChatId;

            var updateResult = await _userManager.UpdateAsync(existingUser);
            if (!updateResult.Succeeded)
            {
                var errors = string.Join("; ", updateResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"User creation failed: {errors}");
            }

            return;
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
            ChatId = request.ChatId,
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
    }

    public async Task SendSmsCodeAsync(SendSmsCodeRequest sendSmsCodeRequest)
    {
        ArgumentNullException.ThrowIfNull(nameof(sendSmsCodeRequest));

        var smsCode = new Random().Next(1000, 9999).ToString();
        _smsCodeService.SaveCode(sendSmsCodeRequest.PhoneNumber, smsCode);

        var messsage = new SmsMessage(sendSmsCodeRequest.PhoneNumber, "Это тест от Eskiz");
        await _smsService.SendMessage(messsage);
    }

    public async Task SendCodeForTelegramAsync(SendCodeTelegramRequest sendCodeTelegramRequest)
    {
        ArgumentNullException.ThrowIfNull(nameof(sendCodeTelegramRequest));

        var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == sendCodeTelegramRequest.PhoneNumber)
            ?? throw new UserNameNotExistException($"Phone Number: {sendCodeTelegramRequest.PhoneNumber} does not exist. Re-register!");

        var smsCode = new Random().Next(1000, 9999).ToString();
        _smsCodeService.SaveCode(sendCodeTelegramRequest.PhoneNumber, smsCode);

        var message = sendCodeTelegramRequest.Language switch
        {
            "uz" => $"Tasdiqlovchi kod: {smsCode}",
            "ru" => $"Код подтверждения: {smsCode}",
            "en" => $"Verification code: {smsCode}",
            _ => $"Tasdiqlovchi kod: {smsCode}"
        };
        await _telegramService.SendMessageAsync(existingUser.ChatId, message);
    }

    public async Task<string> LoginForAdminAsync(LoginForAdminRequest request)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(nameof(request));

        var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == request.Login)
            ?? throw new UserNameNotExistException($"Login: {request.Login} does not exist.");

        var isPasswordValid = await _userManager.CheckPasswordAsync(existingUser, request.Password);
        if (!isPasswordValid)
        {
            throw new InvalidPasswordException("The provided password is incorrect.");
        }

        var roles = await _userManager.GetRolesAsync(existingUser);

        var token = _jwtTokenHandler.GenerateToken(existingUser, roles);

        return token;
    }

    public async Task<UserDto> GetUserInfoAsync()
    {
        var userId = _currentUserService.GetUserId();

        var user = await _context.Users
            .AsNoTracking()
            .Include(u => u.Bookings.Where(b => b.Success))
            .FirstOrDefaultAsync(x => x.Id == userId)
            ?? throw new EntityNotFoundException($"User is not exist.");

        var userDto = new UserDto(
            user.FirstName,
            user.LastName,
            user.PhoneNumber ?? "",
            user.Bookings
                .Where(b => b.Success) 
                .Select(b => new BookingDto(
                    b.BookingCode,
                    b.ServiceName, 
                    b.BranchName,  
                    b.StartDate,
                    b.StartTime.ToString()
                ))
                .ToList()
        );

        return userDto;
    }
}
