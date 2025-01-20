using Bookify.Application.Requests.Auth;

namespace Bookify.Application.Interfaces;

public interface IAuthService
{
    Task<string> LoginAsync(LoginRequest loginRequest);
    Task RegisterAsync(RegisterRequest request);
    Task SendSmsCodeAsync(SendSmsCodeRequest sendSmsCodeRequest);
    Task SendCodeForTelegramAsync(SendCodeTelegramRequest sendCodeTelegramRequest);
    //Task ConfirmEmailAsync(EmailConfirmationRequest request);
    //Task ResetPasswordAsync(ResetPasswordRequest request);
    //Task ConfirmResetPasswordAsync(ConfirmResetPasswordRequest request);
}
