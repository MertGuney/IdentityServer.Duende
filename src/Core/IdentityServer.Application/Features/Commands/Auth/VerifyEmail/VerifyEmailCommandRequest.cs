namespace IdentityServer.Application.Features.Commands.Auth.VerifyEmail;

public class VerifyEmailCommandRequest : IRequest<ResponseModel<NoContentModel>>
{
    public string Code { get; set; }
    public string Email { get; set; }
}
