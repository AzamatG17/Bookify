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
    private readonly IAesEncryptionService _aesEncryptionService;

    public AuthService(
        IApplicationDbContext context,
        UserManager<User> userManager,
        IJwtTokenHandler jwtTokenHandler,
        ISmsCodeService smsCodeService,
        ISmsService smsService,
        ITelegramService telegramService,
        ICurrentUserService currentUserService,
        IAesEncryptionService aesEncryptionService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _jwtTokenHandler = jwtTokenHandler ?? throw new ArgumentNullException(nameof(jwtTokenHandler));
        _smsCodeService = smsCodeService ?? throw new ArgumentNullException(nameof(smsCodeService));
        _smsService = smsService ?? throw new ArgumentNullException(nameof(smsService));
        _telegramService = telegramService ?? throw new ArgumentNullException(nameof(telegramService));
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        _aesEncryptionService = aesEncryptionService ?? throw new ArgumentNullException(nameof(aesEncryptionService));
    }

    public async Task<string> LoginAsync(Requests.Auth.LoginRequest loginRequest)
    {
        ArgumentNullException.ThrowIfNull(loginRequest);

        var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginRequest.PhoneNumber)
            ?? throw new UserNameNotExistException($"Номер телефона: {loginRequest.PhoneNumber} не существует.");

        if (!_smsCodeService.ValidateCode(loginRequest.PhoneNumber, loginRequest.Code))
        {
            throw new SmsCodeValidationException("Код недействителен или просрочен. Повторите попытку.");
        }

        var roles = await _userManager.GetRolesAsync(existingUser);

        var token = _jwtTokenHandler.GenerateToken(existingUser, roles);

        return token;
    }

    public async Task<string> LoginForTelegramAsync(LoginForTelegramRequest loginForTelegramRequest)
    {
        ArgumentNullException.ThrowIfNull(loginForTelegramRequest);

        var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginForTelegramRequest.PhoneNumber)
            ?? throw new UserNameNotExistException($"Номер телефона: {loginForTelegramRequest.PhoneNumber} не существует.");

        if (existingUser.ChatId != loginForTelegramRequest.ChatId)
        {
            throw new ChatIdValidationException("Указанный ChatId недействителен для данного номера телефона.");
        }

        var roles = await _userManager.GetRolesAsync(existingUser);

        var token = _jwtTokenHandler.GenerateToken(existingUser, roles);

        return token;
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == request.PhoneNumber);
        if (existingUser is not null)
        {
            existingUser.PhoneNumber = request.PhoneNumber;
            existingUser.FirstName = request.FirstName;
            existingUser.LastName = request.LastName;
            existingUser.ChatId = request.ChatId;
            existingUser.Language = request.Language;

            var updateResult = await _userManager.UpdateAsync(existingUser);
            if (!updateResult.Succeeded)
            {
                var errors = string.Join("; ", updateResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Не удалось создать пользователя: {errors}");
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
            Language = request.Language,
        };

        var createResult = await _userManager.CreateAsync(newUser);
        if (!createResult.Succeeded)
        {
            var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Не удалось создать пользователя: {errors}");
        }

        var addToRoleResult = await _userManager.AddToRoleAsync(newUser, RoleConsts.User);
        if (!addToRoleResult.Succeeded)
        {
            var errors = string.Join("; ", addToRoleResult.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Не удалось назначить роль «Пользователь»: {errors}");
        }
    }

    public async Task SendSmsCodeAsync(SendSmsCodeRequest sendSmsCodeRequest)
    {
        ArgumentNullException.ThrowIfNull(sendSmsCodeRequest);

        var smsCode = new Random().Next(1000, 9999).ToString();
        _smsCodeService.SaveCode(sendSmsCodeRequest.PhoneNumber, smsCode);

        var messsage = new SmsMessage(sendSmsCodeRequest.PhoneNumber, "Это тест от Eskiz");
        await _smsService.SendMessage(messsage);
    }

    public async Task SendCodeForTelegramAsync(SendCodeTelegramRequest sendCodeTelegramRequest)
    {
        ArgumentNullException.ThrowIfNull(sendCodeTelegramRequest);

        var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == sendCodeTelegramRequest.PhoneNumber)
            ?? throw new UserNameNotExistException($"Номер телефона: {sendCodeTelegramRequest.PhoneNumber} не существует. Перерегистрируйтесь!");

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
        ArgumentNullException.ThrowIfNull(request);

        var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == request.Login)
            ?? throw new UserNameNotExistException($"Логин: {request.Login} не существует.");

        var isPasswordValid = await _userManager.CheckPasswordAsync(existingUser, request.Password);
        if (!isPasswordValid)
        {
            throw new InvalidPasswordException("Предоставленный пароль неверный.");
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
            .Include(u => u.ETickets.Where(e => e.Success))
                .ThenInclude(s => s.Service)
                    .ThenInclude(b => b.Branch)
            .Include(u => u.ETickets.Where(e => e.Success))
                .ThenInclude(ub => ub.ServiceRating)
            .Include(u => u.Bookings.Where(b => b.Success))
                .ThenInclude(ub => ub.ServiceRating)
            .Include(u => u.Bookings.Where(b => b.Success))
                .ThenInclude(s => s.Service)
                    .ThenInclude(b => b.Branch)
            .FirstOrDefaultAsync(x => x.Id == userId)
            ?? throw new EntityNotFoundException($"Пользователь не существует.");

        return MapToUserDto(user);
    }

    public async Task<UserDto> GetUserInfoWithChatId(int chatId)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Include(u => u.ETickets.Where(e => e.Success))
                .ThenInclude(s => s.Service)
                    .ThenInclude(b => b.Branch)
            .Include(u => u.ETickets.Where(e => e.Success))
                .ThenInclude(ub => ub.ServiceRating)
            .Include(u => u.Bookings.Where(b => b.Success))
                .ThenInclude(ub => ub.ServiceRating)
            .FirstOrDefaultAsync(x => x.ChatId == chatId)
            ?? throw new EntityNotFoundException($"Пользователь не существует.");

        return MapToUserDto(user);
    }

    public async Task<UserDto> GetUserInfoWithChatIdAndTokenId(int chatId, string tokenId)
    {
        var token = await _aesEncryptionService.Decrypt(tokenId);
        if (!token)
        {
            throw new InvalidTokenException("Расшифрованный токен недействителен.");
        }

        return await GetUserInfoWithChatId(chatId);
    }

    public static UserDto MapToUserDto(User user)
    {
        return new UserDto(
                user.FirstName,
                user.LastName,
                user.UserName ?? "",
                user.Bookings?
                    .Where(b => b != null)
                    .Select(b => new BookingWithIdsDto(
                        b.Id,
                        b.BookingId,
                        b.BookingCode,
                        b.ServiceName,
                        b.Service?.BranchId ?? 0,
                        b.Service?.Branch?.BranchId ?? 0,
                        b.BranchName,
                        b.StartDate,
                        b.StartTime.ToString()
                    ))
                    .ToList() ?? new List<BookingWithIdsDto>(),
                user.ETickets?
                    .Where(e => e != null)
                    .Select(e => new ETicketDto(
                        e.TicketId,
                        e.Number,
                        e.Message,
                        e.ServiceName,
                        e.Service?.BranchId ?? 0,
                        e.Service?.Branch?.BranchId ?? 0,
                        e.Service.Branch.Projects,
                        e.BranchName,
                        e.BranchName,
                        e.ValidUntil
                    ))
                    .ToList() ?? new List<ETicketDto>()
            );
    }
}
