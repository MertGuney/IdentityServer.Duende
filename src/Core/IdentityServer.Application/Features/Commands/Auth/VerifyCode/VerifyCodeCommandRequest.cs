namespace IdentityServer.Application.Features.Commands.Auth.VerifyCode;

public class VerifyCodeCommandRequest : IRequest<ResponseModel<NoContentModel>>
{
    public string Code { get; set; }
    public string Email { get; set; }
    public CodeTypeEnum CodeType { get; set; }
}
