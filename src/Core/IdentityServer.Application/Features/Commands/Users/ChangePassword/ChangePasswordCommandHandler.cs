namespace IdentityServer.Application.Features.Commands.Users.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommandRequest, ResponseModel<NoContentModel>>
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    private readonly ICodeService _codeService;

    public ChangePasswordCommandHandler(IUserService userService, IAuthService authService, ICodeService codeService)
    {
        _userService = userService;
        _authService = authService;
        _codeService = codeService;
    }

    public async Task<ResponseModel<NoContentModel>> Handle(ChangePasswordCommandRequest request, CancellationToken cancellationToken)
    {
        User user = await _userService.GetAsync();

        var isVerified = await _codeService.VerifyAsync(user.Id, request.Code, CodeTypeEnum.ChangePassword, cancellationToken);
        if (!isVerified) return await ResponseModel<NoContentModel>.FailureAsync(1, "Unverified code", "Code unverified", StatusCodes.Status400BadRequest);

        var changePasswordResult = await _authService.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!changePasswordResult.IsSuccessful) return changePasswordResult;

        return await _authService.UpdateSecurityStampAsync(user);
    }
}