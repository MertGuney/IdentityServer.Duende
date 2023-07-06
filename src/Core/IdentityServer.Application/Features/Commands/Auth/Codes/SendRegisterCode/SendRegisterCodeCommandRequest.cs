namespace IdentityServer.Application.Features.Commands.Auth.Codes.SendRegisterCode;

public class SendRegisterCodeCommandRequest : IRequest<ResponseModel<NoContentModel>>
{
    public string Email { get; set; }
}
