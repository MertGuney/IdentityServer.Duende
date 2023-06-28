namespace IdentityServer.Application.Features.Commands.Auth.TFAs.Enable;

public class EnableTFACommandResponse
{
    public string FormattedKey { get; set; }
    public string AuthenticatorKey { get; set; }

    public EnableTFACommandResponse(string formattedKey, string authenticatorKey)
    {
        FormattedKey = formattedKey;
        AuthenticatorKey = authenticatorKey;
    }
}
