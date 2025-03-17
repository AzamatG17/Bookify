using Bookify.Application.DTOs;
using Bookify.Application.Requests.Auth;

namespace Bookify.Application.Interfaces.IServices;

public interface IAuthService
{
    Task<string> LoginAsync(LoginRequest loginRequest);
    Task<string> LoginForTelegramAsync(LoginForTelegramRequest loginForTelegramRequest);
    Task RegisterAsync(RegisterRequest request);
    Task SendSmsCodeAsync(SendSmsCodeRequest sendSmsCodeRequest);
    Task SendCodeForTelegramAsync(SendCodeTelegramRequest sendCodeTelegramRequest);
    Task<string> LoginForAdminAsync(LoginForAdminRequest request);
    Task<UserDto> GetUserInfoAsync();
    Task<UserDto> GetUserInfoWithChatId(int chatId);
    Task<UserDto> GetUserInfoWithChatIdAndTokenId(int chatId, string tokenId);
}
