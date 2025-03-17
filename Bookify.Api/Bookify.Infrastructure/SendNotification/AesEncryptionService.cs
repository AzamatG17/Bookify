using Bookify.Application.Configurations;
using Bookify.Application.Interfaces;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Bookify.Infrastructure.SendNotification;

public class AesEncryptionService : IAesEncryptionService
{
    private readonly EncryptionOptions _encryptionOptions;
    private readonly BotTokenOptions _botTokenOptions;

    public AesEncryptionService(IOptionsMonitor<EncryptionOptions> encryptionOptions, IOptionsMonitor<BotTokenOptions> botTokenOptions)
    {
        _encryptionOptions = encryptionOptions.CurrentValue ?? throw new ArgumentNullException(nameof(encryptionOptions));
        _botTokenOptions = botTokenOptions.CurrentValue ?? throw new ArgumentNullException(nameof(botTokenOptions));
    }

    public async Task<bool> Decrypt(string cipherText)
    {
        byte[] encryptedData = Convert.FromBase64String(cipherText);

        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_encryptionOptions.Key);
        aes.IV = Encoding.UTF8.GetBytes(_encryptionOptions.IV);

        using var decryptor = aes.CreateDecryptor();
        using var ms = new MemoryStream(encryptedData);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        var decryptedToken = await sr.ReadToEndAsync();

        int lastDashIndex = decryptedToken.LastIndexOf("S5S");
        if (lastDashIndex == -1) return false;

        string token = decryptedToken[..lastDashIndex];
        string timestamp = decryptedToken[(lastDashIndex + 3)..];

        if (!DateTime.TryParseExact(timestamp, "yyyy-MM-ddTHH:mm:ss.fffZ",
            CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var tokenTime))
        {
            return false;
        }

        if (DateTime.UtcNow > tokenTime.AddMinutes(1))
        {
            return false;
        }

        return token == _botTokenOptions.TelegramToken;
    }
}



