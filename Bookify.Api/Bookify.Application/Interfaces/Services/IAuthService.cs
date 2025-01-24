using Bookify.Application.Requests.Auth;

namespace Bookify.Application.Interfaces.IServices;

public interface IAuthService
{
    Task<string> LoginAsync(LoginRequest loginRequest);
    Task RegisterAsync(RegisterRequest request);
    Task SendSmsCodeAsync(SendSmsCodeRequest sendSmsCodeRequest);
    Task SendCodeForTelegramAsync(SendCodeTelegramRequest sendCodeTelegramRequest);
    Task<string> LoginForAdminAsync(LoginForAdminRequest request);
}
