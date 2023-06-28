namespace IdentityServer.Application.Services.Abstractions;

/// <summary>
/// Two-Factor Authentication
/// </summary>
public interface ITFAService
{
    Task<bool> IsEnabledAsync(User user);

    Task<string> GetAuthenticatorKeyAsync(User user);

    Task<bool> VerifyTwoFactorAuthCodeAsync(User user, string code);

    Task<ResponseModel<NoContentModel>> ResetAuthenticatorKeyAsync(User user);

    Task<ResponseModel<NoContentModel>> SetTwoFactorEnabledAsync(User user, bool enabled);
}
