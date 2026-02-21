using Microsoft.AspNetCore.DataProtection;

namespace UI_Test.Services;

public class EncryptedConfigurationService(IDataProtectionProvider dataProtectionProvider) : IEncryptedConfigurationService
{
    private readonly IDataProtector _protector = dataProtectionProvider.CreateProtector("UI-Test.CriticalSettings.v1");

    public string Encrypt(string value) => _protector.Protect(value);

    public string? TryDecrypt(string? encryptedValue)
    {
        if (string.IsNullOrWhiteSpace(encryptedValue))
        {
            return null;
        }

        try
        {
            return _protector.Unprotect(encryptedValue);
        }
        catch
        {
            return null;
        }
    }
}
