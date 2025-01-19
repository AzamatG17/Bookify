using Bookify.Application.Requests.Auth;

namespace Bookify.Application.Interfaces;

public interface IAuthService
{
    Task<string> LoginAsync(Requests.Auth.LoginRequest loginRequest);
    Task<string> RegisterAsync(Requests.Auth.RegisterRequest request);
    Task SendSmsCodeAsync(SendSmsCodeRequest sendSmsCodeRequest);
    //Task ConfirmEmailAsync(EmailConfirmationRequest request);
    //Task ResetPasswordAsync(ResetPasswordRequest request);
    //Task ConfirmResetPasswordAsync(ConfirmResetPasswordRequest request);
}
