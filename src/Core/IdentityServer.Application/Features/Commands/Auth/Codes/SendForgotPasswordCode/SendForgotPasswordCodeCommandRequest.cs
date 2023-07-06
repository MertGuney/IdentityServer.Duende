namespace IdentityServer.Application.Features.Commands.Auth.Codes.SendForgotPasswordCode;

public class SendForgotPasswordCodeCommandRequest : IRequest<ResponseModel<NoContentModel>>
{
    public string Email { get; set; }
}
