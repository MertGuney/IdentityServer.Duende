namespace IdentityServer.Application.Features.Commands.Auth.TFAs.Activate;

public class ActivateTFACommandRequest : IRequest<ResponseModel<NoContentModel>>
{
    public string Code { get; set; }
}
