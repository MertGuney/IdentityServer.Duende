namespace IdentityServer.Infrastructure.Services;
public class AuthService : IAuthService
{
    private readonly IMailService _mailService;
    private readonly ILogger<AuthService> _logger;
    private readonly UserManager<User> _userManager;

    public AuthService(IMailService mailService, ILogger<AuthService> logger, UserManager<User> userManager)
    {
        _mailService = mailService;
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<ResponseModel<NoContentModel>> RegisterAsync(User user, string password)
    {
        IdentityResult result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            List<ErrorModel> errors = new();
            foreach (var error in result.Errors)
            {
                errors.Add(new ErrorModel(1, error.Code, error.Description));
                _logger.LogWarning($"An error occurred while creating the user. User: {user.Email} Code: {error.Code} Message: {error.Description}");
            }
            return await ResponseModel<NoContentModel>.FailureAsync(errors, StatusCodes.Status400BadRequest);
        }
        return await ResponseModel<NoContentModel>.SuccessAsync(StatusCodes.Status201Created);
    }

    public async Task<ResponseModel<NoContentModel>> AddToRoleAsync(User user, string role)
    {
        IdentityResult result = await _userManager.AddToRoleAsync(user, role);
        if (!result.Succeeded)
        {
            List<ErrorModel> errors = new();
            foreach (var error in result.Errors)
            {
                errors.Add(new ErrorModel(1, error.Code, error.Description));
                _logger.LogWarning($"An error occurred while adding the role to the user. User: {user.Email} Code: {error.Code} Message: {error.Description}");
            }
            return await ResponseModel<NoContentModel>.FailureAsync(errors, StatusCodes.Status400BadRequest);
        }
        return await ResponseModel<NoContentModel>.SuccessAsync();
    }

    public async Task<bool> SendEmailConfirmationTokenAsync(User user)
    {
        var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = CryptographyExtensions.UrlEncode(confirmationToken);

        return await _mailService.SendEmailConfirmationMailAsync(user.Email, user.Id.ToString(), encodedToken);
    }

    public async Task<ResponseModel<NoContentModel>> ForgotPasswordAsync(string email)
    {
        User user = await _userManager.FindByEmailAsync(email);
        if (user is null) return ResponseModel<NoContentModel>.UserNotFound();

        var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = CryptographyExtensions.UrlEncode(resetPasswordToken);

        return await _mailService.SendResetPasswordMailAsync(user.Email, user.Id.ToString(), encodedToken)
            ? await ResponseModel<NoContentModel>.SuccessAsync()
            : ResponseModel<NoContentModel>.FailedToSendEmail();
    }

    public async Task<ResponseModel<NoContentModel>> ResetPasswordAsync(string userId, string encodedToken, string newPassword)
    {
        User user = await _userManager.FindByIdAsync(userId);
        if (user is null) return ResponseModel<NoContentModel>.UserNotFound();

        var token = encodedToken.UrlDecode();
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

        if (!result.Succeeded)
        {
            List<ErrorModel> errors = new();
            foreach (var error in result.Errors)
            {
                errors.Add(new ErrorModel(1, error.Code, error.Description));
                _logger.LogWarning($"An error occurred while resetting the password. User: {user.Email} Code: {error.Code} Message: {error.Description}");
            }
            return await ResponseModel<NoContentModel>.FailureAsync(errors, StatusCodes.Status400BadRequest);
        }
        return await ResponseModel<NoContentModel>.SuccessAsync();
    }
}
