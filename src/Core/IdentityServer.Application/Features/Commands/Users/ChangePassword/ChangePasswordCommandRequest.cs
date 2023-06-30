namespace IdentityServer.Application.Features.Commands.Users.ChangePassword;

public class ChangePasswordCommandRequest : IRequest<ResponseModel<NoContentModel>>
{
    public string Code { get; set; }
    public string NewPassword { get; set; }
    public string CurrentPassword { get; set; }
}