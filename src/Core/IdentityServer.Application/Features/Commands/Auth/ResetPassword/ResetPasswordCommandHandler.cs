namespace IdentityServer.Application.Features.Commands.Auth.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommandRequest, ResponseModel<NoContentModel>>
{
    private readonly IAuthService _authService;

    public ResetPasswordCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public Task<ResponseModel<NoContentModel>> Handle(ResetPasswordCommandRequest request, CancellationToken cancellationToken)
    {
        return _authService.ResetPasswordAsync(request.UserId, request.Token, request.NewPassword);
    }
}
