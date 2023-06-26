namespace IdentityServer.Infrastructure.Validators;

public class TokenExchangeExtensionGrantValidator : IExtensionGrantValidator
{
    private readonly ITokenValidator _tokenValidator;
    public string GrantType => OidcConstants.GrantTypes.TokenExchange;

    public TokenExchangeExtensionGrantValidator(ITokenValidator tokenValidator)
    {
        _tokenValidator = tokenValidator;
    }

    public async Task ValidateAsync(ExtensionGrantValidationContext context)
    {
        var subjectToken = context.Request.Raw.Get(OidcConstants.TokenRequest.SubjectToken);
        if (string.IsNullOrEmpty(subjectToken))
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "token missing");
            return;
        }
        var tokenValidateResult = await _tokenValidator.ValidateAccessTokenAsync(subjectToken);
        if (tokenValidateResult.IsError)
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "token invalid");
            return;
        }
        var subjectClaim = tokenValidateResult.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject);
        if (subjectClaim == null)
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "token must contain sub");
            return;
        }
        context.Result = new GrantValidationResult(subjectClaim.Value, OidcConstants.TokenTypes.AccessToken, tokenValidateResult.Claims);
        return;
    }
}