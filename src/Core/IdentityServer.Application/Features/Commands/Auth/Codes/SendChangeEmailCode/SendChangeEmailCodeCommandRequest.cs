namespace IdentityServer.Application.Features.Commands.Auth.Codes.SendChangeEmailCode;

public class SendChangeEmailCodeCommandRequest : IRequest<ResponseModel<NoContentModel>>
{
    public string Email { get; set; }
    public string NewEmail { get; set; }
}
