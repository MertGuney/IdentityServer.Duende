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

    public async Task<bool> RegisterAsync(string email, string userName, string password)
    {
        User user = new(email, userName);

        IdentityResult result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                _logger.LogWarning($"An error occurred while creating user. User: {email} Code: {error.Code} Message: {error.Description}");
            }
            return false;
        }
        else
        {
            var addToRoleResult = await AddToRoleAsync(user, "Customer");
            if (!addToRoleResult) return false;

            return await SendEmailConfirmationTokenAsync(user);
        }
    }

    public async Task<bool> AddToRoleAsync(User user, string role)
    {
        IdentityResult result = await _userManager.AddToRoleAsync(user, role);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                _logger.LogWarning($"An error occurred while adding the role to the user. Code: {error.Code} Message: {error.Description}");
            }
            return false;
        }
        return true;
    }

    public async Task<bool> SendEmailConfirmationTokenAsync(User user)
    {
        var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = CryptographyExtensions.UrlEncode(confirmationToken);

        return await _mailService.SendEmailConfirmationMailAsync(user.Email, user.Id.ToString(), encodedToken);
    }

    //public async Task<bool> ResetPasswordAsync(string email, string code, string newPassword, CancellationToken cancellationToken)
    //{
    //    User user = await _userManager.FindByEmailAsync(email);
    //    if (user is not null)
    //    {
    //        var verifyCode = await _codeService.IsVerifiedAsync(user.Id, code, cancellationToken);
    //        if (!verifyCode) throw new CustomApplicationException("Verification code could not be verified.");

    //        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

    //        IdentityResult result = await _userManager.ResetPasswordAsync(user, token, newPassword);
    //        if (!result.Succeeded) throw new CustomApplicationException("Reset Password Exception");

    //        IdentityResult securityResult = await _userManager.UpdateSecurityStampAsync(user);
    //        if (!securityResult.Succeeded) throw new CustomApplicationException("Security Stamp Result");

    //        return true;
    //    }
    //    throw new NotFoundException("User Not Found");
    //}
}
