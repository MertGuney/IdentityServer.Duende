namespace IdentityServer.Application.Features.Commands.Auth.SendCode;

public class SendCodeCommandRequest : IRequest<ResponseModel<NoContentModel>>
{
    public string Email { get; set; }
    public CodeTypeEnum CodeType { get; set; }
}