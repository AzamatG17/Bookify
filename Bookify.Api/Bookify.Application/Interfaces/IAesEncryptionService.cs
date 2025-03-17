namespace Bookify.Application.Interfaces;

public interface IAesEncryptionService
{
    Task<bool> Decrypt(string cipherText);
}
