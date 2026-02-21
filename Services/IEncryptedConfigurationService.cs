namespace UI_Test.Services;

public interface IEncryptedConfigurationService
{
    string Encrypt(string value);
    string? TryDecrypt(string? encryptedValue);
}
