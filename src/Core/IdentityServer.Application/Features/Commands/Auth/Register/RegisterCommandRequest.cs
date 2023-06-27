namespace IdentityServer.Application.Features.Commands.Auth.Register;
public class RegisterCommandRequest : IRequest<ResponseModel<NoContentModel>>
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
