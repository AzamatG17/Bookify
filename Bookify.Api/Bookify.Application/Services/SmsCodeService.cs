using Bookify.Application.Interfaces.IServices;
using Microsoft.Extensions.Caching.Memory;

namespace Bookify.Application.Services;

public sealed class SmsCodeService : ISmsCodeService
{
    private readonly IMemoryCache _memoryCache;
    private readonly TimeSpan _expirationTime = TimeSpan.FromMinutes(5);

    public SmsCodeService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public void SaveCode(string phoneNumber, string smsCode)
    {
        _memoryCache.Set(phoneNumber, smsCode, _expirationTime);
    }

    public string GetCode(string phoneNumber)
    {
        _memoryCache.TryGetValue(phoneNumber, out string smsCode);
        return smsCode;
    }

    public bool ValidateCode(string phoneNumber, string enteredCode)
    {
        var storedCode = GetCode(phoneNumber);
        return storedCode != null && storedCode == enteredCode;
    }
}
