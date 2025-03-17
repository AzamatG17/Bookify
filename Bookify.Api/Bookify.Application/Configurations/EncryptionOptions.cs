namespace Bookify.Application.Configurations;

public sealed class EncryptionOptions
{
    public const string SectionName = nameof(EncryptionOptions);

    public string Key { get; set; }
    public string IV { get; set; }
}
