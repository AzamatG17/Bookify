namespace Bookify.Application.Interfaces;

public interface ISmsCodeService
{
    void SaveCode(string phoneNumber, string smsCode);
    string GetCode(string phoneNumber);
    bool ValidateCode(string phoneNumber, string enteredCode);
}
