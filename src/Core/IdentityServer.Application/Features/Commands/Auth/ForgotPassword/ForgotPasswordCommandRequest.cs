namespace IdentityServer.Application.Features.Commands.Auth.ForgotPassword;

public class ForgotPasswordCommandRequest : IRequest<ResponseModel<NoContentModel>>
{
    public string Email { get; set; }
}
