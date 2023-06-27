namespace IdentityServer.Application.Features.Commands.Auth.ResetPassword;

public class ResetPasswordCommandRequest : IRequest<ResponseModel<NoContentModel>>
{
    public string Token { get; set; }
    public string UserId { get; set; }
    public string NewPassword { get; set; }
}
