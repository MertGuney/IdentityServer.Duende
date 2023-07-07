namespace IdentityServer.Application.Features.Commands.Users.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommandRequest, ResponseModel<NoContentModel>>
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public ChangePasswordCommandHandler(IUserService userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    public async Task<ResponseModel<NoContentModel>> Handle(ChangePasswordCommandRequest request, CancellationToken cancellationToken)
    {
        User user = await _userService.GetAsync();

        var changePasswordResult = await _authService.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!changePasswordResult.IsSuccessful) return changePasswordResult;

        return await _authService.UpdateSecurityStampAsync(user);
    }
}