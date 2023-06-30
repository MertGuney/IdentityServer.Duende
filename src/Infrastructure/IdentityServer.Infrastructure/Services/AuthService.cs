namespace IdentityServer.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly UserManager<User> _userManager;

    public AuthService(ILogger<AuthService> logger, UserManager<User> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<ResponseModel<NoContentModel>> ChangePasswordAsync(User user, string currentPassword, string newPassword)
    {
        var isExistPassword = await _userManager.CheckPasswordAsync(user, currentPassword);
        if (!isExistPassword) return await ResponseModel<NoContentModel>.FailureAsync(1, "CurrentPasswordNotMatch", "Current password does not match", StatusCodes.Status404NotFound);

        IdentityResult changePasswordResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        if (!changePasswordResult.Succeeded)
        {
            List<ErrorModel> errors = new();
            foreach (var error in changePasswordResult.Errors)
            {
                errors.Add(new ErrorModel(1, error.Code, error.Description));
                _logger.LogWarning($"An error occurred while changing the password. User: {user.Email} Code: {error.Code} Message: {error.Description}");
            }
            return await ResponseModel<NoContentModel>.FailureAsync(errors, StatusCodes.Status400BadRequest);
        }
        return await ResponseModel<NoContentModel>.SuccessAsync();
    }

    public async Task<ResponseModel<NoContentModel>> ChangeEmailAsync(User user, string newEmail)
    {
        var changeEmailToken = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);

        var changeEmailResult = await _userManager.ChangeEmailAsync(user, newEmail, changeEmailToken);
        if (!changeEmailResult.Succeeded)
        {
            List<ErrorModel> errors = new();
            foreach (var error in changeEmailResult.Errors)
            {
                errors.Add(new ErrorModel(1, error.Code, error.Description));
                _logger.LogWarning($"An error occurred while changing the password. User: {user.Email} Code: {error.Code} Message: {error.Description}");
            }
            return await ResponseModel<NoContentModel>.FailureAsync(errors, StatusCodes.Status400BadRequest);
        }
        return await ResponseModel<NoContentModel>.SuccessAsync();
    }

    public async Task<ResponseModel<NoContentModel>> ConfirmEmailAsync(User user)
    {
        var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var result = await _userManager.ConfirmEmailAsync(user, emailConfirmationToken);
        if (!result.Succeeded)
        {
            List<ErrorModel> errors = new();
            foreach (var error in result.Errors)
            {
                errors.Add(new ErrorModel(1, error.Code, error.Description));
                _logger.LogWarning($"An error occurred while changing the password. User: {user.Email} Code: {error.Code} Message: {error.Description}");
            }
            return await ResponseModel<NoContentModel>.FailureAsync(errors, StatusCodes.Status400BadRequest);
        }
        return await ResponseModel<NoContentModel>.SuccessAsync();
    }

    public async Task<ResponseModel<NoContentModel>> ResetPasswordAsync(User user, string newPassword)
    {
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        IdentityResult resetPasswordResult = await _userManager.ResetPasswordAsync(user, token, newPassword);
        if (!resetPasswordResult.Succeeded)
        {
            List<ErrorModel> errors = new();
            foreach (var error in resetPasswordResult.Errors)
            {
                errors.Add(new ErrorModel(1, error.Code, error.Description));
                _logger.LogWarning($"An error occurred while resetting the password. User: {user.Email} Code: {error.Code} Message: {error.Description}");
            }
            return await ResponseModel<NoContentModel>.FailureAsync(errors, StatusCodes.Status400BadRequest);
        }
        return await ResponseModel<NoContentModel>.SuccessAsync();
    }

    public async Task<ResponseModel<NoContentModel>> UpdateSecurityStampAsync(User user)
    {
        IdentityResult securityStampResult = await _userManager.UpdateSecurityStampAsync(user);
        if (!securityStampResult.Succeeded)
        {
            List<ErrorModel> errors = new();
            foreach (var error in securityStampResult.Errors)
            {
                errors.Add(new ErrorModel(1, error.Code, error.Description));
                _logger.LogWarning($"An error occurred while the security updateding. User: {user.Email} Code: {error.Code} Message: {error.Description}");
            }
            return await ResponseModel<NoContentModel>.FailureAsync(errors, StatusCodes.Status400BadRequest);
        }
        return await ResponseModel<NoContentModel>.SuccessAsync();
    }
}
