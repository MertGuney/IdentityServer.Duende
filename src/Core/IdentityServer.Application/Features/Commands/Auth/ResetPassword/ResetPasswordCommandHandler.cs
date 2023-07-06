namespace IdentityServer.Application.Features.Commands.Auth.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommandRequest, ResponseModel<NoContentModel>>
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    private readonly ICodeService _codeService;

    public ResetPasswordCommandHandler(IUserService userService, IAuthService authService, ICodeService codeService)
    {
        _userService = userService;
        _authService = authService;
        _codeService = codeService;
    }

    public async Task<ResponseModel<NoContentModel>> Handle(ResetPasswordCommandRequest request, CancellationToken cancellationToken)
    {
        User user = await _userService.GetByEmailAsync(request.Email);
        if (user is null) return ResponseModel<NoContentModel>.UserNotFound();

        var isVerifiedCode = await _codeService.VerifyAsync(user.Id, request.Code, CodeTypeEnum.ForgotPassword, cancellationToken);
        if (!isVerifiedCode)
            return await ResponseModel<NoContentModel>.FailureAsync(1, "UnverifiedCode", "Code unverified.", StatusCodes.Status400BadRequest);

        var resetPasswordResult = await _authService.ResetPasswordAsync(user, request.NewPassword);
        if (!resetPasswordResult.IsSuccessful) return resetPasswordResult;

        return await _authService.UpdateSecurityStampAsync(user);
    }
}
