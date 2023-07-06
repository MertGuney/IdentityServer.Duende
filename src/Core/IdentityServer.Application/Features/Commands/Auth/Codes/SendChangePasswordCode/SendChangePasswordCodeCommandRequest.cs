namespace IdentityServer.Application.Features.Commands.Auth.Codes.SendChangePasswordCode;

public class SendChangePasswordCodeCommandRequest : IRequest<ResponseModel<NoContentModel>>
{
    public string Email { get; set; }
}
