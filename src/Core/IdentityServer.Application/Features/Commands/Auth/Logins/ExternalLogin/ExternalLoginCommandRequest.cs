namespace IdentityServer.Application.Features.Commands.Auth.Logins.ExternalLogin;

public class ExternalLoginCommandRequest : IRequest<ExternalLoginCommandResponse>
{
    public string Email { get; set; }
}
