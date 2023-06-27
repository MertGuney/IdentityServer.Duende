namespace IdentityServer.Application.Features.Commands.Auth.ForgotPassword;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommandRequest, ResponseModel<NoContentModel>>
{
    private readonly IAuthService _authService;

    public ForgotPasswordCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<ResponseModel<NoContentModel>> Handle(ForgotPasswordCommandRequest request, CancellationToken cancellationToken)
    {
        return await _authService.ForgotPasswordAsync(request.Email);
    }
}
