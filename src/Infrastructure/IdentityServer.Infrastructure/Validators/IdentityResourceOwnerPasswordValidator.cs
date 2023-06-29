namespace IdentityServer.Infrastructure.Validators;

public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public IdentityResourceOwnerPasswordValidator(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        var signInResult = await _signInManager.PasswordSignInAsync(context.UserName, context.Password, false, true);
        if (!signInResult.Succeeded)
        {
            Dictionary<string, object> errors = new();
            if (signInResult.RequiresTwoFactor)
            {
                var code = context.Request.Raw.Get("code");
                if (string.IsNullOrWhiteSpace(code))
                {
                    errors.Add("errors", new List<string> { "Your account is protected by 2FA, please enter the code." });
                    context.Result.CustomResponse = errors;
                    context.Result.ErrorDescription = "required_auth_code";
                    return;
                }
                var tfaSignInResult = await _signInManager.TwoFactorAuthenticatorSignInAsync(code, false, false);
                if (!tfaSignInResult.Succeeded)
                {
                    errors.Add("errors", new List<string> { "Invalid Authentication Code, please try again." });
                    context.Result.CustomResponse = errors;
                    context.Result.ErrorDescription = "invalid_auth_code";
                    return;
                }
            }
            else if (signInResult.IsLockedOut)
            {
                errors.Add("errors", new List<string> { "Your account is locked out, please try again 5 minutes later." });
                context.Result.CustomResponse = errors;
                context.Result.ErrorDescription = "account_lockedout";
                return;
            }
            else
            {
                errors.Add("errors", new List<string> { "Invalid username or password" });
                context.Result.CustomResponse = errors;
                context.Result.ErrorDescription = "invalid_username_or_password";
                return;
            }
        }
        var existUser = await _userManager.FindByNameAsync(context.UserName);
        context.Result = new GrantValidationResult(existUser.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
    }
}