namespace IdentityServer.Infrastructure.Services;

public class TFAService : ITFAService
{
    private readonly ILogger<TFAService> _logger;
    private readonly UserManager<User> _userManager;

    public TFAService(ILogger<TFAService> logger, UserManager<User> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<bool> IsEnabledAsync(User user)
    {
        return await _userManager.GetTwoFactorEnabledAsync(user);
    }

    public async Task<string> GetAuthenticatorKeyAsync(User user)
    {
        return await _userManager.GetAuthenticatorKeyAsync(user);
    }

    public async Task<bool> VerifyTwoFactorAuthCodeAsync(User user, string code)
    {
        return await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, code);
    }

    public async Task<ResponseModel<NoContentModel>> SetTwoFactorEnabledAsync(User user, bool enabled)
    {
        IdentityResult result = await _userManager.SetTwoFactorEnabledAsync(user, enabled);
        if (!result.Succeeded)
        {
            List<ErrorModel> errors = new();
            foreach (var error in result.Errors)
            {
                errors.Add(new ErrorModel(1, error.Code, error.Description));
                _logger.LogWarning($"An error occurred while enabling two-factor authentication . User: {user.Email} Code: {error.Code} Message: {error.Description}");
            }
            return await ResponseModel<NoContentModel>.FailureAsync(errors, StatusCodes.Status400BadRequest);
        }
        return await ResponseModel<NoContentModel>.SuccessAsync();
    }

    public async Task<ResponseModel<NoContentModel>> ResetAuthenticatorKeyAsync(User user)
    {
        IdentityResult result = await _userManager.ResetAuthenticatorKeyAsync(user);
        if (!result.Succeeded)
        {
            List<ErrorModel> errors = new();
            foreach (var error in result.Errors)
            {
                errors.Add(new ErrorModel(1, error.Code, error.Description));
                _logger.LogWarning($"An error occurred while reseting two-factor authenticator key. User: {user.Email} Code: {error.Code} Message: {error.Description}");
            }
            return await ResponseModel<NoContentModel>.FailureAsync(errors, StatusCodes.Status400BadRequest);
        }
        return await ResponseModel<NoContentModel>.SuccessAsync();
    }
}
